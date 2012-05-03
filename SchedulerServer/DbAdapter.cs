using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Diagnostics;
namespace SchedulerServer
{
    class DbAdapter
    {
        OdbcConnection conn;
        OdbcCommand command;
        OdbcDataReader reader;
        Singleton singleton;
        int rowsNo;
        public DbAdapter()
        {
            singleton = Singleton.Instance;
            string connString = "DRIVER={" + singleton.dbSettings.Driver + "};" +
            "SERVER=" + singleton.dbSettings.Host + ";" +
            "DATABASE=" + singleton.dbSettings.Schema + ";" +
            "UID=" + singleton.dbSettings.Username + ";" +
            "PASSWORD=" + singleton.dbSettings.Password + ";" +
            "OPTION=" + singleton.dbSettings.Option + ";";
            conn = new OdbcConnection(connString);
            conn.Open();
        }
        public OdbcDataReader executeQuery(string query)
        {
            command = new OdbcCommand(query, conn);
            rowsNo = command.ExecuteNonQuery();
            reader = command.ExecuteReader();
            return reader;
        }
        public int affectedRows()
        {
            return rowsNo;
        }
    }
}