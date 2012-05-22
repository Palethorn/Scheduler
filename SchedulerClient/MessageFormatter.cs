﻿using System;
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
    }
}