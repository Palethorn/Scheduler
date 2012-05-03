using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.Odbc;

namespace SchedulerServer
{
    class MessageFormatter
    {
        XmlDocument xdoc;
        XmlDocument header;
        public XmlDocument createHeader(Dictionary<string, string> keyValue)
        {
            header = new XmlDocument();
            XmlElement root = header.CreateElement("message");
            XmlElement headers = header.CreateElement("headers");
            XmlElement key;
            foreach (string k in keyValue.Keys)
            {
                key = header.CreateElement(k);
                key.Value = keyValue[k];
                headers.AppendChild(k);
            }
            root.AppendChild(headers);
            header.AppendChild(root);
            return header;
        }
        public XmlDocument createMessage(string message, string messageType, string errStatus)
        {
            xdoc = new XmlDocument();
            XmlElement root = xdoc.CreateElement("message");
            XmlElement text = xdoc.CreateElement("text");
            text.Value = message;
            XmlAttribute type = xdoc.CreateAttribute("type");
            type.Value = messageType;
            XmlAttribute errorStatus = xdoc.CreateAttribute("error_status");
            errorStatus.Value = errStatus;
            root.Attributes.Append(type);
            root.Attributes.Append(errorStatus);
            root.AppendChild(text);
            return xdoc;
        }
        public XmlDocument formatTasks(OdbcDataReader r)
        {
            xdoc = new XmlDocument();
            XmlElement title, notes, startdatetime, enddatetime, place;
            XmlElement root = xdoc.CreateElement("message");
            XmlAttribute type = xdoc.CreateAttribute("type");
            XmlAttribute errorStatus = xdoc.CreateAttribute("error_status");
            type.Value = "tasks";
            root.Attributes.Append(type);
            XmlElement tasks = xdoc.CreateElement("tasks");
            while (r.Read())
            {
                XmlElement task = xdoc.CreateElement("task");
                title = xdoc.CreateElement("title");
                title.Value = r.GetValue(0).ToString();
                task.AppendChild(title);
                notes = xdoc.CreateElement("notes");
                notes.Value = r.GetValue(1).ToString();
                task.AppendChild(notes);
                startdatetime = xdoc.CreateElement("startdatetime");
                startdatetime.Value = r.GetValue(2).ToString();
                task.AppendChild(startdatetime);
                enddatetime = xdoc.CreateElement("enddatetime");
                enddatetime.Value = r.GetValue(3).ToString();
                task.AppendChild(enddatetime);
                place = xdoc.CreateElement("place");
                place.Value = r.GetValue(4).ToString();
                task.AppendChild(place);
                tasks.AppendChild(task);
            }
            root.AppendChild(tasks);
            errorStatus.Value = "0";
            root.Attributes.Append(errorStatus);
            xdoc.AppendChild(root);
            return xdoc;
        }
    }
}
