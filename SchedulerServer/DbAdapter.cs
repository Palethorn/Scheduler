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
        string connString;
        public DbAdapter()
        {
            singleton = Singleton.Instance;
            connString = "DRIVER={" + singleton.dbSettings.Driver + "};" +
            "SERVER=" + singleton.dbSettings.Host + ";" +
            "DATABASE=" + singleton.dbSettings.Schema + ";" +
            "UID=" + singleton.dbSettings.Username + ";" +
            "PASSWORD=" + singleton.dbSettings.Password + ";" +
            "OPTION=" + singleton.dbSettings.Option + ";";
            conn = new OdbcConnection(connString);
            conn.Open();
        }
        public OdbcDataReader executeQuery(string query, bool altering)
        {
            reader = null;
            rowsNo = 0;
            lock(conn)
            {
                command = conn.CreateCommand();
                if (altering)
                {
                    command.CommandText = "START TRANSACTION";
                    command.ExecuteNonQuery();
                }
                command.CommandText = query;
                rowsNo = command.ExecuteNonQuery();
                if (altering)
                {
                    command.CommandText = "COMMIT";
                    command.ExecuteNonQuery();
                }
                reader = command.ExecuteReader();
            }
                return reader;
        }
        public int affectedRows()
        {
            return rowsNo;
        }
    }
}