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
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace SchedulerClient
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        Singleton singleton;
        DispatcherTimer visibilitySpan;
        DoubleAnimation fadeIn;
        DoubleAnimation fadeOut;
        public PopupWindow()
        {
            InitializeComponent();
            singleton = Singleton.Instance;
            singleton.exitApp += closeThis;
            singleton.popup += popup;
            visibilitySpan = new DispatcherTimer();
            visibilitySpan.Tick += close;
            fadeIn = new DoubleAnimation(0, 0.85, TimeSpan.FromMilliseconds(200), FillBehavior.HoldEnd);
            fadeOut = new DoubleAnimation(0.85, 0, TimeSpan.FromMilliseconds(200), FillBehavior.HoldEnd);
            this.Visibility = Visibility.Collapsed;
            Root.Opacity = 0;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        public void popup(string message, int errorStatus = 1)
        {
            this.Dispatcher.Invoke(new Invoker(() => {
                this.Visibility = Visibility.Visible;
                Message.Text = message;
                if (errorStatus == 1)
                {
                    Message.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    Message.Foreground = new SolidColorBrush(Color.FromArgb(255, 34, 103, 176));
                }
                fadeIn.Completed += new EventHandler((o, a) =>
                {
                    visibilitySpan.Interval = new TimeSpan(0, 0, 0, singleton.popupTime, 0);
                    visibilitySpan.Start();
                });
                Root.BeginAnimation(Border.OpacityProperty, fadeIn);
            }));
        }
        public void close(object sender, EventArgs args)
        {
            visibilitySpan.Stop();
            fadeOut.Completed += new EventHandler((o, a) =>
            {
                    this.Visibility = Visibility.Collapsed;
            });
            Root.BeginAnimation(Border.OpacityProperty, fadeOut);
        }
        public void closeThis()
        {
            this.Close();
        }
    }
}
