using System;
using System.Windows;
using System.Windows.Controls;
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
        private readonly SolidColorBrush _pumpkin = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 117, 24));
        private readonly SolidColorBrush _lime = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 255, 0));
        private readonly SolidColorBrush _cornFlowerBlue = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 100, 149, 237));

        const string ToastLongMsg = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";
        
        public Prompts()
        {
            InitializeComponent();
            DataContext = this;
        }

		private void Toast_Click(object sender, RoutedEventArgs e)
		{
			var toast = new ToastPrompt {
                Title = "Basic",
                Message = ToastLongMsg,
            };
			toast.Show();
		}

        private void ToastWithImgAndNoTitle_Click(object sender, RoutedEventArgs e)
		{
		    var toast = new ToastPrompt
                               {
                                   Message = ToastLongMsg,
                                   ImageSource = new BitmapImage(new Uri("..\\media\\c4f_26x26.png", UriKind.RelativeOrAbsolute))
                               };
            toast.Show();
		}

        private void ToastWithImg_Click(object sender, RoutedEventArgs e)
        {
            var toast = new ToastPrompt
            {
                Title = "With Image",
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                Message = ToastLongMsg,
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

        private void ToastWrap_Click(object sender, RoutedEventArgs e)
        {
            var toast = new ToastPrompt
            {
                TextWrapping = TextWrapping.Wrap,
                Title = "Basic",
                Message = ToastLongMsg,
            };
            toast.Show();
        }

        private void ToastWithImgAndNoTitleWrap_Click(object sender, RoutedEventArgs e)
        {
            var toast = new ToastPrompt
            {
                TextWrapping = TextWrapping.Wrap,
                Message = ToastLongMsg,
                ImageSource = new BitmapImage(new Uri("..\\media\\c4f_26x26.png", UriKind.RelativeOrAbsolute))
            };
            toast.Show();
        }

        private void ToastWithImgWrap_Click(object sender, RoutedEventArgs e)
        {
            var toast = new ToastPrompt
            {
                TextWrapping = TextWrapping.Wrap,
                Title = "With Image",
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                Message = ToastLongMsg,
                ImageSource = new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute))
            };
            toast.Show();
        }

        private void ToastAdvancedWrap_Click(object sender, RoutedEventArgs e)
        {
            var toast = new ToastPrompt
            {
                TextWrapping = TextWrapping.Wrap,
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
            resultBlock.Text = e.PopUpResult.ToString();
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
            passwordInput.Completed += stringObject_Completed;

            passwordInput.Show();
        }
        
        private void PasswordNoEnter_Click(object sender, RoutedEventArgs e)
        {
            var passwordInput = new PasswordInputPrompt
            {
                Title = "Enter won't submit",
                Message = "Enter key won't submit now",
                IsSubmitOnEnterKey = false
            };
            passwordInput.Completed += stringObject_Completed;

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

            passwordInput.Completed += stringObject_Completed;
            passwordInput.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.TelephoneNumber } } };
            passwordInput.Value = "doom";
            passwordInput.Show();
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt
                            {
                                Title = "Basic Input",
                                Message = "I'm a basic input prompt",
                            };
            input.Completed += stringObject_Completed;
            
            input.Show();
        }

        private void InputNoEnter_Click(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt
                            {
                                Title = "Enter won't submit",
                                Message = "Enter key won't submit now",
                                IsSubmitOnEnterKey = false
                            };
            input.Completed += stringObject_Completed;
            
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

            input.Completed += stringObject_Completed;
            
            input.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.TelephoneNumber } } };
            input.Show();
        }

        private void Message_Click(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
            {
                Title = "Basic Message",
                Message = "I'm a basic message prompt.  Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor. Cras sodales, erat eu varius elementum, augue turpis consequat tellus, ut tempor arcu urna hendrerit leo. Fusce molestie convallis nunc, ac porttitor diam porta ut. ",
            };
            messagePrompt.Completed += stringObject_Completed;

            messagePrompt.Show();
        }

        private void MessageAdvanced_Click(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
            {
                Title = "Advanced Message",
                Message = "When complete, i'll navigate back",
                Overlay = _cornFlowerBlue,
                IsCancelVisible = true
            };

			messagePrompt.Completed += stringObject_Completed;
    
            messagePrompt.Show();
        }


        private void MessageCustom_Click(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
                                    {
                                        Title = "Advanced Message",
                                        Background = _lime,
                                        Foreground = _pumpkin,
                                        Overlay = _cornFlowerBlue,
                                        IsCancelVisible = true,
                                        
                                    };

            var btn = new Button { Content = "Msg Box" };
            btn.Click += (s, ee) => resultBlock.Text = "Hi!";
            messagePrompt.Body = btn;

            messagePrompt.Completed += stringObject_Completed;

            messagePrompt.Show();
        }

            
        private void MessageSuper_Click(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
                                    {
                                        Title = "Advanced Message",
                                        Background = _lime,
                                        Foreground = _pumpkin,
                                        Overlay = _cornFlowerBlue,
                                    };

            var button1 = new RoundButton { Content = "Hide" };
            button1.Click += (o, arg) => messagePrompt.Hide();

            var button2 = new RoundButton { Content = "Complete" };
            button2.Click += (o, arg) => messagePrompt.OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.Ok, Result = "You clicked the Complete Button" });

            messagePrompt.ActionPopUpButtons.Clear();
            messagePrompt.ActionPopUpButtons.Add(button1);
            messagePrompt.ActionPopUpButtons.Add(button2);

            messagePrompt.Completed += stringObject_Completed;

            messagePrompt.Show();
        }

        void baseObject_Completed(object sender, PopUpEventArgs<object, PopUpResult> e)
        {
            resultBlock.Text = GetBaseResult(e.PopUpResult);
        }

        void stringObject_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            resultBlock.Text = GetBaseResult(e.PopUpResult) + e.Result;
        }

        private string GetBaseResult(PopUpResult popUp)
        {
            var result = "meh?";

            if (popUp == PopUpResult.Ok)
                result = "OK!";
            else if (popUp == PopUpResult.Cancelled)
                result = "CANCELLED!";

            return result;
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

		private void navToStress_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Samples/PromptStressTest.xaml", UriKind.Relative));
		}
     }
}