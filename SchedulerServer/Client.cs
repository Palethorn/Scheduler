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
namespace SchedulerServer
{
    class Client
    {
        Singleton singleton;
        TcpClient client;
        NetworkStream clientStream;
        DbAdapter adapter;
        XDocument xdoc;
        BackgroundWorker backWorker;
        string sessionId;
        public Client(TcpClient client)
        {
            singleton = Singleton.Instance;
            singleton.stopSockets += closeSocket;
            backWorker = new BackgroundWorker();
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
            ASCIIEncoding encoder = new ASCIIEncoding();
            clientStream.Read(stream, 0, 4096);
            c = clientStream.Read(stream, 0, 4096);
            asciiString = encoder.GetString(stream, 0, c);
            singleton.log(asciiString);
            string content_length = "";
            try
            {
                xdoc = XDocument.Parse(asciiString);
                if(xdoc.Element("message").Attribute("type").Value == "header")
                {
                    content_length = xdoc.Element("message").Element("headers").Element("content_length").Value;
                    readMessages(Int32.Parse(content_length));
                }
            }
            catch (Exception e)
            {
                singleton.log("Invalid header.");
                return false;
            }
            return true;
        }
        public void readMessages(int contentSize)
        {
            byte[] stream = new byte[contentSize];
            string asciiString = "";
            int c = 0;
            ASCIIEncoding encoder = new ASCIIEncoding();
            clientStream.Read(stream, 0, contentSize);
            c = clientStream.Read(stream, 0, contentSize);
            asciiString = encoder.GetString(stream, 0, c);
            singleton.log(asciiString);
            try
            {
                xdoc = XDocument.Parse(asciiString);
                if (xdoc.Element("message").Attribute("type").Value == "login_request")
                {
                    login(xdoc.Element("message").Element("user_login").Element("email").Value, xdoc.Element("message").Element("user_login").Element("password").Value);
                }
            }
            catch (Exception e)
            {
                singleton.log("Invalid xml.");
            }
        }
        public void login(string email, string password)
        {
            OdbcDataReader r = adapter.executeQuery("select email, password from user where email=\'" + email + "\' and password=\'" + password + "\'");
            ASCIIEncoding encoder = new ASCIIEncoding();
            if (adapter.affectedRows() == 1)
            {
                while(r.Read())
                {
                    SHA256 sha = new SHA256Managed();
                    byte[] result;
                    string hashInput = r.GetValue(0).ToString() + r.GetValue(1).ToString() + DateTime.Now.ToString();
                    singleton.log(hashInput);
                    result = sha.ComputeHash(encoder.GetBytes(hashInput));
                    sessionId = Convert.ToBase64String(result);
                }
            }
        }
        public void addSchedule()
        {
        
        }
        public void closeSocket()
        {
            client.Close();
        }
    }
}
