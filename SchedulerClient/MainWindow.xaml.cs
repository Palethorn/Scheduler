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
using System.Diagnostics;
namespace SchedulerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client client;
        public MainWindow()
        {
            InitializeComponent();
            client = new Client("127.0.0.1", 50555);
            Activated += changeFocusParams;
            Deactivated += changeFocusParams;
            LoginPage loginPage = new LoginPage();
            this.LayoutRoot.Children.Add(loginPage);
        }
        public void MinimizeWin(object sender, MouseButtonEventArgs args)
        {
            this.WindowState = WindowState.Minimized;
        }
        public void CloseApp(object sender, MouseButtonEventArgs args)
        {
            this.Close();
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
                Title.Foreground = new SolidColorBrush(Color.FromArgb(255, 73, 60, 45));
            }
        }
    }
}
