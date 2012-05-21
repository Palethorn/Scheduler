﻿using System;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class RegisterPage : UserControl
    {
        public RegisterPage()
        {
            InitializeComponent();
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
                if (tb.Name == "EmailInput" && tb.Text == "Email")
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
    }
}
