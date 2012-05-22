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
        string messageType;
        string sessionId;
        MessageFormatter messageFormatter;
        public Client(string ip, int port):base()
        {
            singleton = Singleton.Instance;
            singleton.login += loginAction;
            encoder = new ASCIIEncoding();
            messageFormatter = new MessageFormatter();
            Connect(IPAddress.Parse(ip), port);
            clientStream = this.GetStream();
        }
        public void readHeader()
        {
            while (true)
            {
                byte[] buffer = new byte[4096];
                int c = clientStream.Read(buffer, 0, 4096);
                if (c == 0)
                {
                    return;
                }
                string h = Encoding.UTF8.GetString(buffer);
                XDocument header = XDocument.Parse(h.Replace("\0", ""));
                if (header.Element("message").Attribute("type").Value == "header")
                {
                    XElement headers = header.Element("message").Element("headers");
                    XDocument xdoc = readMessage(Int32.Parse(headers.Element("content_length").Value));
                    if (headers.Element("message_type").Value == "tasks")
                    {
                        singleton.addTasks(xdoc);
                    }
                    if (headers.Element("message_type").Value == "popup")
                    {
                        singleton.popup(xdoc);
                    }
                    if (headers.Element("message_type").Value == "login_status")
                    {
                        if (xdoc.Element("message").Element("login_response").Element("status").Value == "ok")
                        {
                            singleton.loginCompleted();
                        }
                    }
                }
            }
        }
        public void loginAction(XDocument xdoc)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("content_length", xdoc.ToString().Length.ToString());
            dict.Add("request_type", "login_request");
            XDocument header = messageFormatter.createHeader(dict);
            sendMessage(header);
            sendMessage(xdoc);
        }
        public XDocument readMessage(int count)
        {
            byte[] buffer = new byte[count];
            clientStream.Read(buffer, 0, count);
            string s = encoder.GetString(buffer);
            XDocument xdoc = XDocument.Parse(s);
            return xdoc;
        }
        public void sendMessage(XDocument message)
        {
            byte[] mbytes = encoder.GetBytes(message.ToString());
            clientStream.Write(mbytes, 0, mbytes.Length);
        }
        public void close()
        {
            this.Close();
        }
    }
}
