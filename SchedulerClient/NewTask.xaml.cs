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

namespace SchedulerClient
{
    /// <summary>
    /// Interaction logic for NewTask.xaml
    /// </summary>
    public partial class NewTask : Window
    {
        Singleton singleton;
        MessageFormatter formatter;
        public NewTask()
        {
            InitializeComponent();
            singleton = Singleton.Instance;
            formatter = new MessageFormatter();
            Activated += changeFocusParams;
            Deactivated += changeFocusParams;
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
        public void taskSubmit(object sender, RoutedEventArgs args)
        {
            DateTime d1 = (DateTime)BeginDateInput.SelectedDate;
            DateTime d2 = (DateTime)EndDateInput.SelectedDate;
            Task t = new Task()
            {
                Title = TitleInput.Text,
                StartTimeDate = d1.ToString("DDMMYYYYhhmmss"),
                EndTimeDate = d2.ToString("DDMMYYYYhhmmss"),
                Notes = NotesInput.Text,
                Place = PlaceInput.Text
            };
            XDocument xdoc = formatter.formatTask(t);
            singleton.newTaskEvent(xdoc);
        }
    }
}
