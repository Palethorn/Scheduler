using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerClient
{
    class Singleton
    {
        private static Singleton instance;
        private Singleton()
        {
            popupTime = 2;
        }
        public DataTranferEvent addTasks;
        public MessagePopupEvent popup;
        public DataTranferEvent login;
        public Event loginCompleted;
        public Event exitApp;
        public Event Connect;
        public Event showRegister;
        public DataTranferEvent newTaskEvent;
        public DataTranferEvent editTaskEvent;
        public DataTranferEvent removeTaskEvent;
        public DataTranferEvent registerEvent;
        public int popupTime;

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
    }
}
