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
        { }
        public DataTranferEvent addTasks;
        public DataTranferEvent getTasks;
        public DataTranferEvent popup;
        public DataTranferEvent login;
        public Event loginCompleted;
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
