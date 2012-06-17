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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace SchedulerClient
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class Tasks : UserControl
    {
        NewTask newTaskWindow;
        DoubleAnimation scrollAnimation;
        Singleton singleton;
        EditTask editTask;
        bool scrolling;
        int space;
        public Tasks()
        {
            InitializeComponent();
            singleton = Singleton.Instance;
            newTaskWindow = new NewTask();
            newTaskWindow.Visibility = Visibility.Collapsed;
            scrollAnimation = new DoubleAnimation();
            scrollAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100));
            scrolling = false;
            this.MouseWheel += scroll;
            space = 0;
            editTask = new EditTask();
            editTask.Visibility = Visibility.Collapsed;
        }
        public void openNewTaskWindow(object sender, RoutedEventArgs args)
        {
            newTaskWindow.Visibility = Visibility.Visible;
        }
        public void scroll(object sender, MouseWheelEventArgs args)
        {
            if (!scrolling)
            {
                if (args.Delta < 120)
                {
                    scrollAnimation.From = Canvas.GetTop(tasksItemsControl);
                    if (space < tasksItemsControl.Items.Count)
                    {
                        scrollAnimation.To = Canvas.GetTop(tasksItemsControl) - 98;
                        space++;
                    }
                }
                else
                {
                    scrollAnimation.From = Canvas.GetTop(tasksItemsControl);
                    if (space > 0)
                    {
                        scrollAnimation.To = Canvas.GetTop(tasksItemsControl) + 98;
                        space--;
                    }
                }
                scrollAnimation.Completed += new EventHandler((o, a) =>
                {
                    scrolling = false;
                });
                scrolling = true;
                tasksItemsControl.BeginAnimation(Canvas.TopProperty, scrollAnimation);
            }
        }
        public void ShowTask(object sender, EventArgs args)
        {
            Canvas c = sender as Canvas;
            editTask.bind(c.DataContext as Task);
            editTask.Visibility = Visibility.Visible;
        }
    }
}
