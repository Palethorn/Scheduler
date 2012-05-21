using System.Xml.Linq;
namespace SchedulerClient
{
    public delegate void Event();
    public delegate void DataTranferEvent(XDocument xdoc);
}
