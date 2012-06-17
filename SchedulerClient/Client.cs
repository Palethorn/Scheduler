using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Xml.Linq;
using System.Threading;

namespace SchedulerClient
{
    class Client : TcpClient
    {
        NetworkStream clientStream;
        Singleton singleton;
        ASCIIEncoding encoder;
        string sessionId;
        MessageFormatter messageFormatter;
        public Client(string ip, int port):base()
        {
            singleton = Singleton.Instance;
            singleton.login += loginAction;
            singleton.loginCompleted += getTasks;
            singleton.newTaskEvent += submitTask;
            singleton.removeTaskEvent += removeTask;
            singleton.editTaskEvent += editTask;
            singleton.registerEvent += registerAction;
            encoder = new ASCIIEncoding();
            messageFormatter = new MessageFormatter();
            Connect(IPAddress.Parse(ip), port);
            clientStream = this.GetStream();
        }
        public void getTasks()
        {
            Dictionary<string, string> h = new Dictionary<string, string>();
            h.Add("request_type", "tasks_request");
            XDocument headers = messageFormatter.createHeader(h);
            sendHeader(headers);
        }
        public void submitTask(XDocument t)
        {
            Dictionary<string, string> h = new Dictionary<string, string>();
            h.Add("content_length", t.ToString().Length.ToString());
            h.Add("request_type", "new_task");
            XDocument header = messageFormatter.createHeader(h);
            sendHeader(header);
            sendMessage(t);
        }
        public void removeTask(XDocument t)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("content_length", t.ToString().Length.ToString());
            dict.Add("request_type", "remove_task");
            XDocument header = messageFormatter.createHeader(dict);
            sendHeader(header);
            sendMessage(t);
        }
        public void editTask(XDocument xdoc)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("content_length", xdoc.ToString().Length.ToString());
            dict.Add("request_type", "edit_task");
            XDocument header = messageFormatter.createHeader(dict);
            sendHeader(header);
            sendMessage(xdoc);
        }
        public void readHeader()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int c = 0;
                while (c < 1024)
                {
                    c += clientStream.Read(buffer, 0, 1024);
                    if (c == 0)
                    {
                        return;
                    }
                }
                XDocument xdoc = new XDocument();
                string h = Encoding.UTF8.GetString(buffer);
                h = h.Replace("\0", "");
                h = h.Replace("  ", "");
                h = h.Replace(" <", "<");
                XDocument header = new XDocument();
                try
                {
                    header = XDocument.Parse(h);
                    if (header.Element("message").Attribute("type").Value == "header")
                    {
                        XElement headers = header.Element("message").Element("headers");
                        if (headers.Element("content_length") != null)
                        {
                            xdoc = readMessage(Int32.Parse(headers.Element("content_length").Value));
                        }
                        if (headers.Element("message_type").Value == "tasks")
                        {
                            singleton.addTasks(xdoc);
                        }
                        if (headers.Element("message_type").Value == "popup")
                        {
                            dispatchPopup(xdoc);
                        }
                        if (headers.Element("message_type").Value == "login_status")
                        {
                            if (xdoc.Element("message").Element("login_response").Element("status").Value == "ok")
                            {
                                sessionId = xdoc.Element("message").Element("login_response").Element("status").Value;
                                singleton.loginCompleted();
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }
        public void dispatchPopup(XDocument xdoc)
        {
            singleton.popup(xdoc.Element("message").Element("text").Value, Int32.Parse(xdoc.Element("message").Attribute("error_status").Value));
        }
        public void loginAction(XDocument xdoc)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("content_length", xdoc.ToString().Length.ToString());
            dict.Add("request_type", "login_request");
            XDocument header = messageFormatter.createHeader(dict);
            sendHeader(header);
            sendMessage(xdoc);
        }
        public void registerAction(XDocument xdoc)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("content_length", xdoc.ToString().Length.ToString());
            dict.Add("request_type", "register_request");
            XDocument header = messageFormatter.createHeader(dict);
            sendHeader(header);
            sendMessage(xdoc);
        }
        public XDocument readMessage(int count)
        {
            byte[] buffer = new byte[count];
            int c = 0;
            while (c < count)
            {
                c += clientStream.Read(buffer, 0, count);
                if (c == 0)
                {
                    return new XDocument();
                }
            }
            string s = Encoding.UTF8.GetString(buffer, 0, count);
            s = s.Replace("\0", "");
            s = s.Replace("  ", "");
            s = s.Replace(" <", "<");
            XDocument xdoc = XDocument.Parse(s);
            return xdoc;
        }
        public void sendHeader(XDocument message)
        {
            string messageString = message.ToString();
            if (messageString.Length < 1024)
            {
                String emptyString = new String(' ', 1024 - messageString.Length);
                messageString += emptyString;
            }
            byte[] mbytes = Encoding.UTF8.GetBytes(messageString);
            clientStream.Write(mbytes, 0, mbytes.Length);
        }
        public void sendMessage(XDocument message)
        {
            string messageString = message.ToString();
            byte[] mbytes = Encoding.UTF8.GetBytes(messageString);
            clientStream.Write(mbytes, 0, mbytes.Length);
        }
        public void close()
        {
            this.Close();
        }
    }
}
