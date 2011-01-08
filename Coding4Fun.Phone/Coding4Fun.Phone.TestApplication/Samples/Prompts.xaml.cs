using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Coding4Fun.Phone.Controls;
using Coding4Fun.Phone.Site.Controls;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Prompts : PhoneApplicationPage
    {
        public Prompts()
        {
            InitializeComponent();
            DataContext = this;
        }
        
        private void About_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutPrompt();
            about.Completed += about_Completed;
            about.Show();
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt();
            input.Completed += input_Completed;
            input.Title = "Basic Input";
            input.Message = "I'm a basic input prompt";
            input.Show();
        }

        private void InputEmail_Click(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt();
            input.Completed += input_Completed;
            input.Title = "TelephoneNum";
            input.Message = "I'm a message about Telephone numbers!";
            input.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.TelephoneNumber } } };
            input.Show();
        }

        void about_Completed(object sender, PopUpEventArgs<object> e)
        {
            if (e.PopUpResult == PopUpResult.OK)
                MessageBox.Show("OK!");
            else if (e.PopUpResult == PopUpResult.Cancelled)
                MessageBox.Show("CANCELLED!");
            else
                MessageBox.Show("meh?");
        }

        void input_Completed(object sender, PopUpEventArgs<string> e)
        {
            if (e.PopUpResult == PopUpResult.OK)
                MessageBox.Show("OK!  " + e.Result);
            else if (e.PopUpResult == PopUpResult.Cancelled)
                MessageBox.Show("CANCELLED!  " + e.Result);
            else
                MessageBox.Show("meh?  " + e.Result);
        }

        private void Ding_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DING!", "dong", MessageBoxButton.OKCancel);
        }

        private void C4F_Click(object sender, RoutedEventArgs e)
        {
            var about = new Coding4FunAboutPrompt();
            about.Show("Clint Rutkas", "ClintRutkas", "Clint@Rutkas.com", "http://betterthaneveryone.com");
        }
     }
}