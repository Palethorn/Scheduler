using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace SchedulerClient
{
    class TaskItemsControl : ItemsControl
    {
        ObservableCollection<Task> tasks;
        public TaskItemsControl()
        {
            tasks = new ObservableCollection<Task>();
            this.ItemsSource = tasks;
        }
        public void parseTasks()
        {
            
        }
    }
}
