using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.Data.Odbc;
using System.Security.Cryptography;
using System.Xml;
using System.IO;
namespace SchedulerServer
{
    class Client
    {
        Singleton singleton;
        TcpClient client;
        NetworkStream clientStream;
        DbAdapter adapter;
        XDocument xdoc;
        MessageFormatter formatter;
        StreamWriter sw;
        string sessionId;
        string userId;
        string cacheFolder;
        public Client(TcpClient client)
        {
            singleton = Singleton.Instance;
            singleton.stopSockets += closeSocket;
            formatter = new MessageFormatter();
            this.client = client;
            logIt();
            clientStream = client.GetStream();
            adapter = new DbAdapter();
            while(readHeader());
        }
        public void logIt()
        {
            IPEndPoint clientEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            singleton.log("Client connected.\nIP address:" + clientEndPoint.Address.ToString() + "\nPort:" + clientEndPoint.Port);
        }
        public bool readHeader()
        {
            byte[] stream = new byte[1024];
            string asciiString = "";
            int c = 0;
            try
            {
                while (c < 1024)
                {
                    c += clientStream.Read(stream, 0, 1024);
                    if (c == 0)
                    {
                        singleton.log("Client " + userId + " disconected");
                        client.Close();
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

            asciiString = Encoding.UTF8.GetString(stream, 0, 1024);
            singleton.log(asciiString);
            string content_length = "";
            string reqType = "";
            
            try
            {
                asciiString = asciiString.Replace("\0", "");
                asciiString = asciiString.Replace("  ", "");
                asciiString = asciiString.Replace(" <", "<");
                XDocument header = XDocument.Parse(asciiString);
                
                if(header.Element("message").Attribute("type").Value == "header")
                {
                    reqType = header.Element("message").Element("headers").Element("request_type").Value;
                    
                    if (reqType == "login_request")
                    {
                        content_length = header.Element("message").Element("headers").Element("content_length").Value;
                        xdoc = readMessage(Int32.Parse(content_length));
                        login(xdoc);
                    }
                    if (reqType == "tasks_request")
                    {
                        getTasks();
                    }
                    if (reqType == "new_task")
                    {
                        content_length = header.Element("message").Element("headers").Element("content_length").Value;
                        xdoc = readMessage(Int32.Parse(content_length));
                        addTask(xdoc);
                    }
                }
            }
            catch (Exception e)
            {
                singleton.log("Invalid header.");
                //Dictionary<string, string> keyValue = new Dictionary<string,string>();
                //XDocument error = formatter.createMessage("Invalid request", "error", "1");
                //keyValue.Add("content_length", error.ToString().Length.ToString());
                //keyValue.Add("message_type", "error");
                //XDocument header = formatter.createHeader(keyValue);
                //sendHeader(header);
                //sendMessage(error);
                return true;
            }
            
            return true;
        }
        public void login(XDocument xdoc)
        {
            XElement email = xdoc.Element("message").Element("user_login").Element("email");
            XElement password = xdoc.Element("message").Element("user_login").Element("password");
            
            OdbcDataReader r = adapter.executeQuery("select email, password, iduser from user where email=\'" + email.Value + "\' and password=\'" + password.Value + "\'");
            
            Dictionary<string, string> dict = new Dictionary<string, string>();
            XDocument doc = new XDocument();
            XDocument header;
            XElement root = new XElement("message");
            XElement response = new XElement("login_response");
            XElement status;
            XElement session_id;

            if (adapter.affectedRows() == 1)
            {
                while(r.Read())
                {
                    SHA512 sha = new SHA512Managed();
                    byte[] result;
                    string hashInput = r.GetValue(0).ToString() + r.GetValue(1).ToString() + DateTime.Now.ToString();
                    singleton.log(hashInput);
                    result = sha.ComputeHash(Encoding.UTF8.GetBytes(hashInput));
                    sessionId = Convert.ToBase64String(result);
                    userId = r.GetValue(2).ToString();

                    status = new XElement("status", "ok");
                    session_id = new XElement("session_id", sessionId);
                    response.Add(status);
                    response.Add(session_id);
                }
                cacheFolder = "../../../cache/" + userId + "/";
            }
            else
            {
                status = new XElement("status", "error");
                response.Add(status);
            }
                root.Add(response);
                doc.Add(root);
                dict.Add("content_length", doc.ToString().Length.ToString());
                dict.Add("message_type", "login_status");
                header = formatter.createHeader(dict);
                sendHeader(header);
                sendMessage(doc);
        }
        public void addTask(XDocument xdoc)
        {
            XDocument message;
            XDocument header;
            Dictionary<string, string> headers = new Dictionary<string,string>();
            
            string title = xdoc.Element("message").Element("task").Element("title") != null ? xdoc.Element("message").Element("task").Element("title").Value : "\'\'";
            string notes = xdoc.Element("message").Element("task").Element("notes") != null ? xdoc.Element("message").Element("task").Element("notes").Value : "\'\'";
            string startdatetime = xdoc.Element("message").Element("task").Element("startdatetime") != null ? xdoc.Element("message").Element("task").Element("startdatetime").Value : "\'\'";
            string enddatetime = xdoc.Element("message").Element("task").Element("enddatetime") != null ? xdoc.Element("message").Element("task").Element("enddatetime").Value : "\'\'";
            string place = xdoc.Element("message").Element("task").Element("place") != null ? xdoc.Element("message").Element("task").Element("place").Value : "\'\'";
            
            OdbcDataReader r = adapter.executeQuery("insert into scheduler.task(title, notes, startdatetime, enddatetime, place) values(\'" + title + "\', \'" + notes + "\', \'" + startdatetime + "\', \'" + enddatetime + "\', \'" + place + "\')");
            updateCache(xdoc.Element("message").Element("task"));
            if (adapter.affectedRows() > 0)
            {
                message = formatter.createMessage("Task added.", "popup", "0");
                headers.Add("content_length", message.ToString().Length.ToString());
                headers.Add("message_type", "popup");
            }
            else
            {
                message = formatter.createMessage("Error while adding task.", "error", "1");
                headers.Add("content_length", message.ToString().Length.ToString());
                headers.Add("message_type", "error");
            }
            
            header = formatter.createHeader(headers);
            sendHeader(header);
            sendMessage(message);
        }
        public void updateCache(XElement task)
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(cacheFolder + "tasks.xml");
                xdoc.Element("message").Element("tasks").Add(task);
                writeCache(xdoc.ToString(), cacheFolder + "tasks.xml");
            }
            catch(Exception e)
            {}
        }
        public void getTasks()
        {
            XDocument header;
            XDocument doc;
            if (!File.Exists(cacheFolder + "tasks.xml"))
            {
                OdbcDataReader r = adapter.executeQuery("select title, notes, startdatetime, enddatetime, place from task where fkuser=" + userId);

                if (adapter.affectedRows() > 0)
                {
                    doc = formatter.formatTasks(r);
                    writeCache(doc.ToString(), cacheFolder + "tasks.xml");
                }
                else
                {
                    doc = formatter.createMessage("No tasks added.", "error", "1");
                }
            }
            else
            {
                try
                {
                    doc = XDocument.Load(cacheFolder + "tasks.xml");
                }
                catch (Exception e)
                {
                    doc = formatter.createMessage("Could not retrieve tasks", "error", "1");
                }
            }
            Dictionary<string, string> keyValue = new Dictionary<string, string>();
            keyValue.Add("content_length", doc.ToString().Length.ToString());
            keyValue.Add("message_type", "tasks");
            header = formatter.createHeader(keyValue);
            sendHeader(header);
            sendMessage(doc);
        }
        public void writeCache(string content, string filename)
        {
            if (!Directory.Exists(cacheFolder))
            {
                Directory.CreateDirectory(cacheFolder);
            }
            sw = new StreamWriter(filename, false);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }
        public XDocument readMessage(int contentSize)
        {
            byte[] stream = new byte[contentSize];
            string asciiString = "";
            int c = 0;
            while (c < contentSize)
            {
                c += clientStream.Read(stream, 0, contentSize);
                if (c == 0)
                {
                    return new XDocument();
                }
            }
            asciiString = Encoding.UTF8.GetString(stream, 0, contentSize);
            singleton.log(asciiString);

            try
            {
                asciiString = asciiString.Replace("\0", "");
                asciiString = asciiString.Replace("  ", "");
                asciiString = asciiString.Replace(" <", "<");
                xdoc = XDocument.Parse(asciiString);
            }
            catch (Exception e)
            {
                singleton.log("Invalid xml.");
            }

            return xdoc;
        }
        public void sendHeader(XDocument header)
        {
            string headerString = header.ToString();
            if (headerString.Length < 1024)
            {
                String emptyString = new String(' ', 1024 - headerString.Length);
                headerString += emptyString;
            }
            byte[] mbytes = Encoding.UTF8.GetBytes(headerString);
            clientStream.Write(mbytes, 0, mbytes.Length);
        }
        public void sendMessage(XDocument message)
        {
            string messageString = message.ToString();
            byte[] mbytes = Encoding.UTF8.GetBytes(messageString);
            clientStream.Write(mbytes, 0, mbytes.Length);
        }
        public void closeSocket()
        {
            client.Close();
        }
    }
}
