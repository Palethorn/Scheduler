using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SchedulerClient
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
        public XDocument formatTask(Task t, string messageType = "new_task")
        {
            xdoc = new XDocument();
            XElement root = new XElement("message");
            XAttribute type = new XAttribute("type", messageType);
            root.Add(type);
            XElement task = new XElement("task");
            XElement id = new XElement("id", t.Id);
            XElement title = new XElement("title", t.Title);
            XElement startdatetime = new XElement("startdatetime", t.StartTimeDate);
            XElement enddatetime = new XElement("enddatetime", t.EndTimeDate);
            XElement notes = new XElement("notes", t.Notes);
            XElement place = new XElement("place", t.Place);
            task.Add(id);
            task.Add(title);
            task.Add(startdatetime);
            task.Add(enddatetime);
            task.Add(notes);
            task.Add(place);
            root.Add(task);
            xdoc.Add(root);
            return xdoc;
        }
    }
}
