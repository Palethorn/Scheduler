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
using System.Xml.Linq;

namespace SchedulerClient
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        Singleton singleton;
        public LoginPage()
        {
            InitializeComponent();
            singleton = Singleton.Instance;
            singleton.loginCompleted += hide;
            this.SetValue(UserControl.WidthProperty, 300.0);
            this.SetValue(UserControl.HeightProperty, 300.0);
            this.SetValue(Canvas.TopProperty, 30.0);
            this.SetValue(Canvas.LeftProperty, 100.0);

            EmailInput.GotFocus += clearInput;
            PasswordInput.GotFocus += clearInput;
            EmailInput.LostFocus += resetInput;
            PasswordInput.LostFocus += resetInput;
        }
        public void hide()
        {
            this.Dispatcher.Invoke(new Invoker(() =>
                {
                    this.Visibility = Visibility.Hidden;
                }));
        }
        void clearInput(object sender, EventArgs args)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                if (tb.Text == "Email")
                {
                    tb.Text = "";
                }
            }
            else
            {
                PasswordBox pb = sender as PasswordBox;
                if(pb.Password == "Password")
                {
                    pb.Password = "";
                }
            }
        }
        void resetInput(object sender, EventArgs args)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                if (tb.Text == "")
                {
                    tb.Text = "Email";
                }
            }
            else
            {
                PasswordBox pb = sender as PasswordBox;
                if (pb.Password == "")
                {
                    pb.Password = "Password";
                }
            }
        }
        public void LoginAction(object sender, RoutedEventArgs args)
        {
            singleton.Connect();
            XDocument xdoc = new XDocument();
            XElement root = new XElement("message");
            XElement userLogin = new XElement("user_login");
            XElement email = new XElement("email", EmailInput.Text);
            XElement password = new XElement("password", PasswordInput.Password);
            userLogin.Add(email);
            userLogin.Add(password);
            root.Add(userLogin);
            root.Add(new XAttribute("message_type", "login_request"));
            xdoc.Add(root);
            singleton.login(xdoc);
        }
    }
}
