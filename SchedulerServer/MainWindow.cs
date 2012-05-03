using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.Threading;
namespace SchedulerServer
{
    public partial class MainWindow : Form
    {
        Singleton singleton;
        Server server;
        NotifyIcon tray;
        XDocument xdoc;
        public MainWindow()
        {
            InitializeComponent();
            this.Icon = new Icon("Icon1.ico");
            singleton = Singleton.Instance;
            singleton.log += appendLog;
            singleton.serverStart += startServer;
            ServerSwitch.Click += switchServer;
            tray = new NotifyIcon();
            tray.Icon = new Icon("Icon1.ico");
            tray.Visible = true;
            tray.MouseClick += trayClicked;
            tray.ContextMenu = new TrayContext();
            singleton.mainWindowVisibilityChanged += toggleMainWindow;
            Application.ApplicationExit += appExit;
            loadServerConfig();
            loadDatabaseConfig();
        }
        public void loadServerConfig()
        {
            xdoc = XDocument.Load("../../../config/server_config.xml");
            BindIpInput.Text = xdoc.Element("config").Element("server_config").Element("bind_ip").Value;
            BindPortInput.Text = xdoc.Element("config").Element("server_config").Element("bind_port").Value;
            singleton.log("Server config loaded.");
        }
        public void loadDatabaseConfig()
        {
            singleton.dbSettings = new DBSettings();
            singleton.log("Database config loaded.");
        }
        public void switchServer(object sender, EventArgs args)
        {
            singleton.toggleServer(!singleton.serverStarted);
            if (singleton.serverStarted == true)
            {
                ServerSwitch.Text = "Stop Server";
            }
            else
            {
                ServerSwitch.Text = "Start Server";
            }
        }
        public void appendLog(string s)
        {
            if (LogTxt.IsDisposed == false)
            {
                if (LogTxt.InvokeRequired)
                {
                    LogTxt.Invoke(new invokeDelegate(() =>
                    {
                        LogTxt.AppendText(s + "\n");
                    }));
                }
                else
                {
                    LogTxt.AppendText(s + "\n");
                }
            }

        }
        public void startServer()
        {
            server = new Server(new IPEndPoint(IPAddress.Parse(BindIpInput.Text), Int32.Parse(BindPortInput.Text)));
        }
        public void trayClicked(object sender, MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            {
                singleton.toggleMainWindow(!singleton.mainWindowVisible);
            }
        }
        public void toggleMainWindow()
        {
            if (singleton.mainWindowVisible)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }
        public void appExit(object sender, EventArgs args)
        {
            server.closeEverything();
            tray.Dispose();
        }
    }
}
