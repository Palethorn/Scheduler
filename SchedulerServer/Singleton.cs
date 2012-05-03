using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulerServer
{
    class Singleton
    {
        private static Singleton instance;
        private Singleton()
        {
            serverStarted = false;
            mainWindowVisible = false;
        }
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
        
        public bool serverStarted;
        public bool mainWindowVisible;
        public Event serverStart;
        public Event serverStop;
        public Event stopSockets;
        public Event mainWindowVisibilityChanged;
        public Log log;
        public DBSettings dbSettings;
        public void toggleServer(bool start = false)
        {
            if (start == true && serverStarted != start)
            {
                serverStart();
                serverStarted = start;
            }
            else if (start == false && serverStarted != start)
            {
                serverStop();
                serverStarted = false;
            }
        }
        public void toggleMainWindow(bool visible = false)
        {
            if (visible != mainWindowVisible)
            {
                mainWindowVisible = visible;
                mainWindowVisibilityChanged();
            }
        }
    }
}
