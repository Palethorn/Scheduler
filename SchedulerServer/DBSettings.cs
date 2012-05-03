using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SchedulerServer
{
    class DBSettings
    {
        XDocument dbConfig;
        Singleton singleton;
        public DBSettings()
        {
            dbConfig = XDocument.Load("../../../config/database_config.xml");
            Driver = dbConfig.Element("config").Element("database_config").Attribute("odbc_driver").Value;
            Schema = dbConfig.Element("config").Element("database_config").Element("schema").Value;
            Host = dbConfig.Element("config").Element("database_config").Element("host").Value;
            Username = dbConfig.Element("config").Element("database_config").Element("user").Value;
            Password = dbConfig.Element("config").Element("database_config").Element("password").Value;
            Option = dbConfig.Element("config").Element("database_config").Attribute("option").Value;
        }
        public string Driver
        {
            get;
            set;
        }
        public string Schema
        {
            get;
            set;
        }
        public string Host
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public string Option
        {
            get;
            set;
        }
    }
}
