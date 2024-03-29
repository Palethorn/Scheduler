﻿using System.Xml.Linq;
namespace SchedulerClient
{
    public delegate void Event();
    public delegate void Invoker();
    public delegate void DataTranferEvent(XDocument xdoc);
    public delegate void MessagePopupEvent(string message, int errorStatus);
}
