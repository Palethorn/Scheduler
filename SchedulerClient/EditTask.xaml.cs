using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Globalization;

namespace SchedulerClient
{
    /// <summary>
    /// Interaction logic for NewTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        Singleton singleton;
        MessageFormatter formatter;
        public EditTask()
        {
            InitializeComponent();
            singleton = Singleton.Instance;
            singleton.exitApp += closeThis;
            formatter = new MessageFormatter();
            Activated += changeFocusParams;
            Deactivated += changeFocusParams;
            RemoveTaskBtn.Click += removeTaskAction;
            SaveTaskBtn.Click += editTaskAction;
        }
        public void MinimizeWin(object sender, MouseButtonEventArgs args)
        {
            this.WindowState = WindowState.Minimized;
        }
        public void CloseApp(object sender, MouseButtonEventArgs args)
        {
            this.Visibility = Visibility.Collapsed;
        }
        public void StartDrag(object sender, MouseButtonEventArgs args)
        {
            this.DragMove();
        }
        public void changeFocusParams(object sender, EventArgs args)
        {
            if (!IsActive)
            {
                Title.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                Title.Foreground = new SolidColorBrush(Color.FromArgb(255, 34, 103, 176));
            }
        }
        public void bind(Task t)
        {
            this.DataContext = t;
            this.BeginDateInput.SelectedDate = DateTime.ParseExact(t.StartTimeDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            this.EndDateInput.SelectedDate = DateTime.ParseExact(t.EndTimeDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        public void removeTaskAction(object sender, EventArgs args)
        {
            Task t = (Task)this.DataContext;
            XDocument xdoc = new XDocument();
            XElement root = new XElement("message");
            XAttribute messageType = new XAttribute("message_type", "remove_task");
            root.Add(messageType);
            XElement task = new XElement("task");
            root.Add(task);
            XElement id = new XElement("id", t.Id);
            task.Add(id);
            xdoc.Add(root);
            singleton.removeTaskEvent(xdoc);
        }
        public void editTaskAction(object sender, EventArgs args)
        {
            Task tsk = (Task)this.DataContext;
            DateTime d1 = (DateTime)BeginDateInput.SelectedDate;
            DateTime d2 = (DateTime)EndDateInput.SelectedDate;
            Task t = new Task()
            {
                Id = tsk.Id,
                Title = TitleInput.Text,
                StartTimeDate = d1.ToString("ddMMyyyyhhmmss"),
                EndTimeDate = d2.ToString("ddMMyyyyhhmmss"),
                Place = PlaceInput.Text,
                Notes = NotesInput.Text
            };
            XDocument xdoc = formatter.formatTask(t, "edit_task");
            singleton.editTaskEvent(xdoc);
        }
        public void closeThis()
        {
            this.Close();
        }
    }
}
