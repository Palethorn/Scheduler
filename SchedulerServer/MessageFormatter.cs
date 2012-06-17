using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.Odbc;
using System.Xml.Linq;

namespace SchedulerServer
{
    class MessageFormatter
    {
        XDocument xdoc;
        XDocument header;
        public XDocument createHeader(Dictionary<string, string> keyValue)
        {
            header = new XDocument();
            XElement root = new XElement("message");
            XAttribute type = new XAttribute("type", "header");
            root.Add(type);
            XElement headers = new XElement("headers");
            XElement key;
            foreach (string k in keyValue.Keys)
            {
                key = new XElement(k, keyValue[k]);
                headers.Add(key);
            }
            root.Add(headers);
            header.Add(root);
            return header;
        }
        public XDocument createMessage(string message, string messageType, string errStatus)
        {
            xdoc = new XDocument();
            XElement root = new XElement("message");
            XElement text = new XElement("text", message);
            XAttribute type = new XAttribute("type", messageType);
            XAttribute errorStatus = new XAttribute("error_status", errStatus);
            root.Add(type);
            root.Add(errorStatus);
            root.Add(text);
            xdoc.Add(root);
            return xdoc;
        }
        public XDocument formatTasks(OdbcDataReader r)
        {
            xdoc = new XDocument();
            XElement title, notes, startdatetime, enddatetime, place, id;
            XElement root = new XElement("message");
            XAttribute type = new XAttribute("type", "tasks");
            XAttribute errorStatus = new XAttribute("error_status", "0");
            root.Add(type);
            XElement tasks = new XElement("tasks");
            while (r.Read())
            {
                XElement task = new XElement("task");
                id = new XElement("id", r.GetValue(0).ToString());
                task.Add(id);
                title = new XElement("title", r.GetValue(1).ToString());
                task.Add(title);
                notes = new XElement("notes", r.GetValue(2).ToString());
                task.Add(notes);
                startdatetime = new XElement("startdatetime", r.GetValue(3).ToString());
                task.Add(startdatetime);
                enddatetime = new XElement("enddatetime", r.GetValue(4).ToString());
                task.Add(enddatetime);
                place = new XElement("place", r.GetValue(5).ToString());
                task.Add(place);
                tasks.Add(task);
            }
            root.Add(tasks);
            root.Add(errorStatus);
            xdoc.Add(root);
            return xdoc;
        }
    }
}
