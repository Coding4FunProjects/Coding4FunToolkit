using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Coding4Fun.Phone.Controls;
using Coding4Fun.Phone.Site.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Prompts : PhoneApplicationPage
    {
        private readonly SolidColorBrush _pumpkin = new SolidColorBrush(Color.FromArgb(255, 255, 117, 24));
        private readonly SolidColorBrush _lime = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
        private readonly SolidColorBrush _cornFlowerBlue = new SolidColorBrush(Color.FromArgb(200, 100, 149, 237));

        public Prompts()
        {
            InitializeComponent();
            DataContext = this;
        }

		private void Toast_Click(object sender, RoutedEventArgs e)
		{
			var toast = new ToastPrompt {Title = " Basic", Message = "Message"};
			toast.Show();
		}

		private void ToastWithImg_Click(object sender, RoutedEventArgs e)
		{
			var toast = new ToastPrompt
			            	{
			            		Title = "With Image",
			            		Message = "Message",
			            		ImageSource = new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute))
			            	};
			toast.Show();
		}

        private void ToastAdvanced_Click(object sender, RoutedEventArgs e)
        {
            var toast = new ToastPrompt
                            {
								IsAppBarVisible = false,
                                Background = _lime,
                                Foreground = _pumpkin,
                                Title = "Advanced",
                                Message = "Custom Fontsize, img, and orientation",
                                FontSize = 50,
                                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                                ImageSource =
                                    new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute))
                            };

            toast.Completed += toast_Completed;
            toast.Show();
        }

        void toast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            MessageBox.Show(e.PopUpResult.ToString());
        }

    	private void About_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutPrompt();
            about.Completed += baseObject_Completed;
            about.Show();
        }

        private void Password_Click(object sender, RoutedEventArgs e)
        {
            var passwordInput = new PasswordInputPrompt
            {
                Title = "Basic Input",
                Message = "I'm a basic input prompt",
            };
            passwordInput.Completed += input_Completed;

            passwordInput.Show();
        }

        private void PasswordAdvanced_Click(object sender, RoutedEventArgs e)
        {
            var passwordInput = new PasswordInputPrompt
            {
                Title = "TelephoneNum",
                Message = "I'm a message about Telephone numbers!",
                Background = _lime,
                Foreground = _pumpkin,
                Overlay = _cornFlowerBlue,
                IsCancelVisible = true
            };

            passwordInput.Completed += input_Completed;

            passwordInput.Show();
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt
                            {
                                Title = "Basic Input",
                                Message = "I'm a basic input prompt",
                            };
            input.Completed += input_Completed;
            
            input.Show();
        }

        private void InputAdvanced_Click(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt
                            {
                                Title = "TelephoneNum",
                                Message = "I'm a message about Telephone numbers!",
                                Background = _lime,
                                Foreground = _pumpkin,
                                Overlay = _cornFlowerBlue,
                                IsCancelVisible = true
                            };

            input.Completed += input_Completed;
            
            input.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.TelephoneNumber } } };
            input.Show();
        }

        private void Message_Click(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
            {
                Title = "Basic Message",
                Message = "I'm a basic message prompt.",
            };
            messagePrompt.Completed += stringObject_Completed;

            messagePrompt.Show();
        }

        private void MessageAdvanced_Click(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
            {
                Title = "Advanced Message",
                Message = "I'm an advanced message prompt!\nI'm a lot of text, blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah",
                Background = _lime,
                Foreground = _pumpkin,
                Overlay = _cornFlowerBlue,
                IsCancelVisible = true
            };

            messagePrompt.Completed += stringObject_Completed;
    
            messagePrompt.Show();
        }

        void baseObject_Completed(object sender, PopUpEventArgs<object, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
                MessageBox.Show("OK!");
            else if (e.PopUpResult == PopUpResult.Cancelled)
                MessageBox.Show("CANCELLED!");
            else
                MessageBox.Show("meh?");
        }

        void stringObject_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
                MessageBox.Show("OK!");
            else if (e.PopUpResult == PopUpResult.Cancelled)
                MessageBox.Show("CANCELLED!");
            else
                MessageBox.Show("meh?");
        }

        void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
                MessageBox.Show("You typed: " + e.Result);
            else if (e.PopUpResult == PopUpResult.Cancelled)
                MessageBox.Show("CANCELLED! " + e.Result);
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