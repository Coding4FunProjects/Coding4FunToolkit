using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            var input = new InputPrompt {Title = "Basic Input", Message = "I'm a basic input prompt"};

            input.Completed += input_Completed;
            
            input.Show();
        }

        private void InputAdvanced_Click(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt
                            {
                                Title = "TelephoneNum",
                                Message = "I'm a message about Telephone numbers!",
                                Background = new SolidColorBrush(Color.FromArgb(255, 100, 149, 237)), //cornflower blue
                                Overlay = new SolidColorBrush(Color.FromArgb(200, 255, 117, 24)) // pumpkin
                            };

            input.Completed += input_Completed;
            
            input.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.TelephoneNumber } } };
            input.Show();
        }

        void about_Completed(object sender, PopUpEventArgs<object, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.OK)
                MessageBox.Show("OK!");
            else if (e.PopUpResult == PopUpResult.Cancelled)
                MessageBox.Show("CANCELLED!");
            else
                MessageBox.Show("meh?");
        }

        void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.OK)
                MessageBox.Show("OK!  " + e.Result);
            else if (e.PopUpResult == PopUpResult.Cancelled)
                MessageBox.Show("CANCELLED!  " + e.Result);
            else
                MessageBox.Show("meh?  " + e.Result);
        }
        
        private void C4F_Click(object sender, RoutedEventArgs e)
        {
            var about = new Coding4FunAboutPrompt();
            about.Show("Clint Rutkas", "ClintRutkas", "Clint@Rutkas.com", "http://betterthaneveryone.com");
        }


        private void Ding_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
        }

        private void GestureListener_Tap(object sender, GestureEventArgs e)
        {
            MessageBox.Show("TAP!", "Testing with Gesture Tap", MessageBoxButton.OKCancel);
        }
     }
}