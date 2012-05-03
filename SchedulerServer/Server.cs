using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;
namespace SchedulerServer
{
    class Server : TcpListener
    {
        private Thread listenThread;
        Singleton singleton;
        List<Thread> threads;
        public Server(IPEndPoint ipEndPoint)
            : base(ipEndPoint)
        {
            singleton = Singleton.Instance;
            singleton.serverStop += closeEverything;
            singleton.log("Server started.\nServer ip: " + ipEndPoint.Address.ToString() + "\nServer port: " + ipEndPoint.Port.ToString());
            threads = new List<Thread>();
            this.Start();
            listenThread = new Thread(new ThreadStart(listen));
            listenThread.Start();
        }
        public void listen()
        {
            while (true)
            {
                TcpClient client = this.AcceptTcpClient();
                Client c = new Client(client);
                Thread t = new Thread(new ParameterizedThreadStart(handleClient));
                threads.Add(t);
                t.Start(c);
            }
        }
        public void handleClient(object client)
        {
            Client c = client as Client;
        }
        public void closeEverything()
        {
            try
            {
                singleton.stopSockets();
            }
            catch (Exception e) {
                // Buzz off!
            }
            foreach (Thread t in threads)
            {
                t.Abort();
            }
            this.Stop();
            listenThread.Abort();
            singleton.log("Server stopped.");
        }
    }
}
