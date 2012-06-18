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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        Singleton singleton;
        public Register()
        {
            InitializeComponent();
            singleton = Singleton.Instance;
            singleton.exitApp += closeThis;
            singleton.showRegister += new Event(() =>
                {
                    this.Visibility = Visibility.Visible;
                });
        }
        public void ClearInput(object sender, EventArgs args)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                if (tb.Name == "NameInput" && tb.Text == "First Name")
                {
                    tb.Text = "";
                }
                if (tb.Name == "LastNameInput" && tb.Text == "Last Name")
                {
                    tb.Text = "";
                }
                if (tb.Name == "EmailInput" && tb.Text == "Email: example@domain.com")
                {
                    tb.Text = "";
                }
            }
            else
            {
                PasswordBox pb = sender as PasswordBox;
                if (pb.Password == "Password")
                {
                    pb.Password = "";
                }
            }
        }
        public void ResetInput(object sender, EventArgs args)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                if (tb.Name == "NameInput" && tb.Text == "")
                {
                    tb.Text = "First Name";
                }
                if (tb.Name == "LastNameInput" && tb.Text == "")
                {
                    tb.Text = "Last Name";
                }
                if (tb.Name == "EmailInput" && tb.Text == "")
                {
                    tb.Text = "Email: example@domain.com";
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
        public void MinimizeWin(object sender, MouseButtonEventArgs args)
        {
            this.WindowState = WindowState.Minimized;
        }
        public void Close(object sender, MouseButtonEventArgs args)
        {
            this.Visibility = Visibility.Collapsed;
        }
        public void StartDrag(object sender, MouseButtonEventArgs args)
        {
            this.DragMove();
        }
        public void register(object sender, RoutedEventArgs args)
        {
            XDocument xdoc = new XDocument();
            XElement root = new XElement("message");
            XAttribute messageType = new XAttribute("type", "register_request");
            root.Add(messageType);
            XElement user = new XElement("user");
            root.Add(user);
            XElement firstName = new XElement("first_name", NameInput.Text);
            user.Add(firstName);
            XElement lastName = new XElement("last_name", LastNameInput.Text);
            user.Add(lastName);
            XElement email = new XElement("email", EmailInput.Text);
            user.Add(email);
            XElement password = new XElement("password", PasswordInput.Password);
            user.Add(password);
            xdoc.Add(root);
            singleton.registerEvent(xdoc);
        }
        public void closeThis()
        {
            this.Close();
        }
    }
}
