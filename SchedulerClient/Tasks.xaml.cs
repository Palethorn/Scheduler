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

namespace SchedulerClient
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class Tasks : UserControl
    {
        NewTask newTaskWindow;
        public Tasks()
        {
            InitializeComponent();
            newTaskWindow = new NewTask();
            newTaskWindow.Visibility = Visibility.Collapsed;
        }
        public void openNewTaskWindow(object sender, RoutedEventArgs args)
        {
            newTaskWindow.Visibility = Visibility.Visible;
        }
    }
}
