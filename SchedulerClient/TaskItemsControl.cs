using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SchedulerClient
{
    class TaskItemsControl : ItemsControl
    {
        ObservableCollection<Task> tasks;
        Singleton singleton;
        public TaskItemsControl()
        {
            singleton = Singleton.Instance;
            singleton.addTasks += parseTasks;
            tasks = new ObservableCollection<Task>();

            this.ItemsSource = tasks;
        }
        public void parseTasks(XDocument xtasks)
        {
            Task t;
            int i = 0;
            foreach (XElement task in xtasks.Element("message").Element("tasks").Descendants("task"))
            {
                t = new Task()
                {
                    Title = task.Element("title") != null ? task.Element("title").Value : "",
                    EndTimeDate = task.Element("enddatetime") != null ? task.Element("enddatetime").Value : "",
                    StartTimeDate = task.Element("startdatetime") != null ? task.Element("startdatetime").Value : "",
                    Notes = task.Element("notes") != null ? task.Element("notes").Value : "",
                    Place = task.Element("place") != null ? task.Element("place").Value : "",
                    Index = i,
                    Top = i * 30
                };
                i++;
                this.Dispatcher.Invoke(new Invoker(() =>
                    {
                        this.tasks.Add(t);
                    }));
            }
        }
    }
}
