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
        ASCIIEncoding encoder;
        MessageFormatter formatter;
        StreamWriter sw;
        string sessionId;
        string userId;
        public Client(TcpClient client)
        {
            singleton = Singleton.Instance;
            singleton.stopSockets += closeSocket;
            encoder = new ASCIIEncoding();
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
            byte[] stream = new byte[4096];
            string asciiString = "";
            int c = 0;
            
            c = clientStream.Read(stream, 0, 4096);
            if (c == 0)
            {
                singleton.log("client " + userId + " disconected");
                return false;
            }
            asciiString = encoder.GetString(stream, 0, c);
            singleton.log(asciiString);
            string content_length = "";
            string reqType = "";
            try
            {
                XDocument header = XDocument.Parse(asciiString);
                if(header.Element("message").Attribute("type").Value == "header")
                {
                    content_length = header.Element("message").Element("headers").Element("content_length").Value;
                    reqType = header.Element("message").Element("headers").Element("request_type").Value;
                    xdoc = readMessage(Int32.Parse(content_length), reqType);
                    if (reqType == "login_request")
                    {
                        login(xdoc);
                    }
                }
            }
            catch (Exception e)
            {
                singleton.log("Invalid header.");
                Dictionary<string, string> keyValue = new Dictionary<string,string>();
                XmlDocument error = formatter.createMessage("Invalid request", "error", "1");
                keyValue.Add("content_length", error.ToString().Length.ToString());
                keyValue.Add("message_type", "error");
                XmlDocument header = formatter.createHeader(keyValue);
                sendHeader(header);
                sendMessage(error);
                return false;
            }
            return true;
        }
        public XDocument readMessage(int contentSize, string reqType)
        {
            byte[] stream = new byte[contentSize];
            string asciiString = "";
            int c = 0;
            //clientStream.Read(stream, 0, contentSize);
            c = clientStream.Read(stream, 0, contentSize);
            asciiString = encoder.GetString(stream, 0, c);
            singleton.log(asciiString);
            try
            {
                xdoc = XDocument.Parse(asciiString);
            }
            catch (Exception e)
            {
                singleton.log("Invalid xml.");
            }
            return xdoc;
        }
        public void login(XDocument xdoc)
        {
            XElement email = xdoc.Element("message").Element("user_login").Element("email");
            XElement password = xdoc.Element("message").Element("user_login").Element("password");
            OdbcDataReader r = adapter.executeQuery("select email, password, iduser from user where email=\'" + email.Value + "\' and password=\'" + password.Value + "\'");
            if (adapter.affectedRows() == 1)
            {
                while(r.Read())
                {
                    SHA512 sha = new SHA512Managed();
                    byte[] result;
                    string hashInput = r.GetValue(0).ToString() + r.GetValue(1).ToString() + DateTime.Now.ToString();
                    singleton.log(hashInput);
                    result = sha.ComputeHash(encoder.GetBytes(hashInput));
                    sessionId = Convert.ToBase64String(result);
                    userId = r.GetValue(2).ToString();
                }
            }
        }
        public void addTask()
        {
        
        }
        public void getTasks()
        {
            XmlDocument header;
            XmlDocument doc;
            OdbcDataReader r = adapter.executeQuery("select title, notes, startdatetime, enddatetime, place from task where fkuser=" + userId);
            if (adapter.affectedRows() > 0)
            {
                doc = formatter.formatTasks(r);
            }
            else
            {
                doc = formatter.createMessage("Could not retrieve tasks", "error", "1");
            }
            Dictionary<string, string> keyValue = new Dictionary<string, string>();
            keyValue.Add("content_length", doc.ToString().Length.ToString());
            keyValue.Add("message_type", "tasks");
            header = formatter.createHeader(keyValue);
            sendHeader(header);
            sendMessage(doc);
            if (!File.Exists("../../../cache/" + userId + "/tasks.xml"))
            {
                writeCache(doc.ToString(), "../../../cache/" + userId + "/tasks.xml");
            }
        }
        public void writeCache(string content, string filename)
        {
            sw = new StreamWriter(filename, false);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }
        public void sendHeader(XmlDocument header)
        {
            byte[] hbytes = encoder.GetBytes(header.ToString());
            clientStream.Write(hbytes, 0, hbytes.Length);
        }
        public void sendMessage(XmlDocument message)
        {
            byte[] mbytes = encoder.GetBytes(message.ToString());
            clientStream.Write(mbytes, 0, mbytes.Length);
        }
        public void closeSocket()
        {
            client.Close();
        }
    }
}
