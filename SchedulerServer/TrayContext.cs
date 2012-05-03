using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SchedulerServer
{
    class TrayContext : ContextMenu
    {
        Singleton singleton;
        public TrayContext()
        {
            singleton = Singleton.Instance;
            this.MenuItems.Add(new MenuItem("Show main window", showMainWindow));
            this.MenuItems.Add(new MenuItem("Start server", startServer));
            this.MenuItems.Add(new MenuItem("Stop server", stopServer));
            this.MenuItems.Add(new MenuItem("Exit", exitApp));
        }
        public void showMainWindow(object sender, EventArgs args)
        {
            singleton.toggleMainWindow(true);
        }
        public void startServer(object sender, EventArgs args)
        {
            singleton.toggleServer(true);
        }
        public void stopServer(object sender, EventArgs args)
        {
            singleton.toggleServer(false);
        }
        public void exitApp(object sender, EventArgs args)
        {
        
        }
    }
}
