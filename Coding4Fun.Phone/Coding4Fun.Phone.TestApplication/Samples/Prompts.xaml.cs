using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Coding4Fun.Phone.Controls;
using Coding4Fun.Phone.Site.Controls;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Prompts : PhoneApplicationPage
    {
		private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 240, 248, 255));
		private readonly SolidColorBrush _naturalBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 135, 189));
		private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 100, 149, 237));

        const string ToastLongMsg = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";
        
        public Prompts()
        {
            InitializeComponent();
        
			DataContext = this;
        }

		#region prompt results
		void PopUpPromptObjectCompleted(object sender, PopUpEventArgs<object, PopUpResult> e)
		{
			resultBlock.Text = e.PopUpResult.ToString();
		}

		void PopUpPromptStringCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			resultBlock.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
		}
		#endregion
		#region toast
		#region basic toast
		private void ToastBasicClick(object sender, RoutedEventArgs e)
		{
			var toast = GetBasicToast();

			toast.Show();
		}

		private void ToastWrapBasicClick(object sender, RoutedEventArgs e)
		{
			var toast = GetBasicToast();
			toast.TextWrapping = TextWrapping.Wrap;

			toast.Show();
		}

    	private static ToastPrompt GetBasicToast(string title = "Basic")
    	{
    		return new ToastPrompt
    		       	{
    		       		Title = title,
    		       		Message = ToastLongMsg,
    		       	};
    	}
		#endregion
		#region toast img and no title
		private void ToastWithImgAndNoTitleClick(object sender, RoutedEventArgs e)
		{
			var toast = GetToastWithImgAndNoTitle();

			toast.Show();
		}

		private void ToastWrapWithImgAndNoTitleClick(object sender, RoutedEventArgs e)
		{
			var toast = GetToastWithImgAndNoTitle();
			toast.TextWrapping = TextWrapping.Wrap;

			toast.Show();
		}

    	private static ToastPrompt GetToastWithImgAndNoTitle()
		{
			return new ToastPrompt
			{
				Message = ToastLongMsg,
				ImageSource = new BitmapImage(new Uri("..\\media\\c4f_26x26.png", UriKind.RelativeOrAbsolute))
			};
		}
    	#endregion
		#region toast img and title
		private void ToastWithImgAndTitleClick(object sender, RoutedEventArgs e)
		{
			var toast = GetToastWithImgAndTitle();

			toast.Show();
		}

		private void ToastWrapWithImgAndTitleClick(object sender, RoutedEventArgs e)
		{
			var toast = GetToastWithImgAndTitle();
			toast.TextWrapping = TextWrapping.Wrap;

			toast.Show();
		}

    	private static ToastPrompt GetToastWithImgAndTitle()
		{
			return new ToastPrompt
			{
				Title = "With Image",
				TextOrientation = System.Windows.Controls.Orientation.Vertical,
				Message = ToastLongMsg,
				ImageSource = new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute))
			};
		}
    	#endregion
		#region toast with custom everything and event
		private void ToastAdvancedClick(object sender, RoutedEventArgs e)
		{
			var toast = GetAdvancedToast();

			toast.Show();
		}

		private void ToastWrapAdvancedClick(object sender, RoutedEventArgs e)
		{
			var toast = GetAdvancedToast();
			toast.TextWrapping = TextWrapping.Wrap;

			toast.Show();
		}

		private ToastPrompt GetAdvancedToast()
		{
			var toast =  new ToastPrompt
			{
				IsAppBarVisible = false,
				Background = _aliceBlueSolidColorBrush,
				Foreground = _cornFlowerBlueSolidColorBrush,
				Title = "Advanced",
				Message = "Custom Fontsize, img, and orientation",
				FontSize = 50,
				TextOrientation = System.Windows.Controls.Orientation.Vertical,
				ImageSource =
					new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute))
			};

			toast.Completed += PopUpPromptStringCompleted;

			return toast;
		}
		#endregion
		#region stress test
		private void ToastSysTrayVisClick(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray();
			GetBasicToast("Test Vis").Show();
		}

		private void ToastSysTrayNotVisClick(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray(false);
			GetBasicToast("Test not Vis").Show();
		}

		private void ToastSysTrayVisWithOpacityClick(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray(true, .8);
			GetBasicToast("Test with Opacity").Show();
		}
		#endregion
		#endregion
		#region about prompt
		private void AboutPromptBlankClick(object sender, RoutedEventArgs e)
        {
            var about = new AboutPrompt();
			about.Completed += PopUpPromptObjectCompleted;
            
			about.Show();
        }

		private void AboutPromptBasicClick(object sender, RoutedEventArgs e)
        {
            var about = new AboutPrompt();
			about.Completed += PopUpPromptObjectCompleted;

			about.Show("Clint Rutkas", "ClintRutkas", "Clint@Rutkas.com", "http://betterthaneveryone.com");
        }

		private void AboutPromptLongClick(object sender, RoutedEventArgs e)
		{
			var about = new AboutPrompt { Title = "Custom Title", VersionNumber = "v3.14159265" };
			about.Completed += PopUpPromptObjectCompleted;

			about.Show(
				new AboutPersonItem {Role="dev" , AuthorName="Clint Rutkas"},
				new AboutPersonItem {Role="site" , WebSiteUrl="http://coding4fun.com"});
		}

		private void AboutPromptC4FClick(object sender, RoutedEventArgs e)
		{
			var about = new Coding4FunAboutPrompt();

			about.Show("Clint Rutkas", "ClintRutkas", "Clint@Rutkas.com", "http://betterthaneveryone.com");
		}
		#endregion
		#region password prompt
		private void PasswordClick(object sender, RoutedEventArgs e)
        {
            var passwordInput = new PasswordInputPrompt
            {
                Title = "Basic Input",
                Message = "I'm a basic input prompt",
            };

            passwordInput.Completed += PopUpPromptStringCompleted;
            
			passwordInput.Show();
        }
        
        private void PasswordNoEnterClick(object sender, RoutedEventArgs e)
        {
            var passwordInput = new PasswordInputPrompt
            {
                Title = "Enter won't submit",
                Message = "Enter key won't submit now",
                IsSubmitOnEnterKey = false
            };

            passwordInput.Completed += PopUpPromptStringCompleted;
            
			passwordInput.Show();
        }

        private void PasswordAdvancedClick(object sender, RoutedEventArgs e)
        {
			var passwordInput = new PasswordInputPrompt
									{
										Title = "TelephoneNum",
										Message = "I'm a message about Telephone numbers!",
										Background = _naturalBlueSolidColorBrush,
										Foreground = _aliceBlueSolidColorBrush,
										Overlay = _cornFlowerBlueSolidColorBrush,
										IsCancelVisible = true,
										InputScope = new InputScope { Names = { new InputScopeName { NameValue = InputScopeNameValue.TelephoneNumber } } },
										Value = "doom"
									};

        	passwordInput.Completed += PopUpPromptStringCompleted; 
			
			passwordInput.Show();
        }
		#endregion
		#region input prompt
		private void InputClick(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt
                            {
                                Title = "Basic Input",
                                Message = "I'm a basic input prompt",
                            };

            input.Completed += PopUpPromptStringCompleted;
            
            input.Show();
        }

        private void InputNoEnterClick(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt
                            {
                                Title = "Enter won't submit",
                                Message = "Enter key won't submit now",
                                IsSubmitOnEnterKey = false
                            };

            input.Completed += PopUpPromptStringCompleted;
            
            input.Show();
        }
        
        private void InputAdvancedClick(object sender, RoutedEventArgs e)
        {
            var input = new InputPrompt
                            {
                                Title = "TelephoneNum",
                                Message = "I'm a message about Telephone numbers!",
								Background = _naturalBlueSolidColorBrush,
								Foreground = _aliceBlueSolidColorBrush,
								Overlay = _cornFlowerBlueSolidColorBrush,
                                IsCancelVisible = true
                            };

            input.Completed += PopUpPromptStringCompleted;
            
            input.InputScope = new InputScope { Names = { new InputScopeName { NameValue = InputScopeNameValue.TelephoneNumber } } };
            input.Show();
        }
		#endregion
		#region message prompt
		private void MessageClick(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
            {
                Title = "Basic Message",
				Message = ToastLongMsg,
            };

            messagePrompt.Completed += PopUpPromptStringCompleted;

            messagePrompt.Show();
        }

        private void MessageAdvancedClick(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
            {
                Title = "Advanced Message",
                Message = "When complete, i'll navigate back",
                Overlay = _cornFlowerBlueSolidColorBrush,
                IsCancelVisible = true
            };

			messagePrompt.Completed += PopUpPromptStringCompleted;
    
            messagePrompt.Show();
        }

        private void MessageCustomClick(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
                                    {
                                        Title = "Custom Body Message",
										Background = _naturalBlueSolidColorBrush,
										Foreground = _aliceBlueSolidColorBrush,
										Overlay = _cornFlowerBlueSolidColorBrush,
                                        IsCancelVisible = true,
                                        
                                    };

            var btn = new Button { Content = "Msg Box" };
            btn.Click += (s, args) => resultBlock.Text = "Hi!";

            messagePrompt.Body = btn;

            messagePrompt.Completed += PopUpPromptStringCompleted;

            messagePrompt.Show();
        }

        private void MessageSuperClick(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
                                    {
                                        Title = "Advanced Message",
										Background = _naturalBlueSolidColorBrush,
										Foreground = _aliceBlueSolidColorBrush,
										Overlay = _cornFlowerBlueSolidColorBrush,
                                    };

            var btnHide = new RoundButton { Content = "Hide" };
            btnHide.Click += (o, args) => messagePrompt.Hide();

            var btnComplete = new RoundButton { Content = "Complete" };
            btnComplete.Click += (o, args) => messagePrompt.OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.Ok, Result = "You clicked the Complete Button" });

            messagePrompt.ActionPopUpButtons.Clear();
            messagePrompt.ActionPopUpButtons.Add(btnHide);
            messagePrompt.ActionPopUpButtons.Add(btnComplete);

            messagePrompt.Completed += PopUpPromptStringCompleted;

            messagePrompt.Show();
        }

		#region stress test
		private void MsgSysTrayVisClick(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray();
			CreateMsgPrompt("Test Vis");
		}

		private void MsgSysTrayNotVisClick(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray(false);
			CreateMsgPrompt("Test not Vis");
		}

		private void MsgSysTrayVisWithOpacityClick(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray(true, .8);
			CreateMsgPrompt("Test with Opacity");
		}

		private static void CreateMsgPrompt(string message)
		{
			var msgPrompt = new MessagePrompt { Title = message, Message = message };
			msgPrompt.Show();
		}
		#endregion
		#endregion

        private void Ding_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
        }

		private void NavToToastStressClick(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Samples/PromptStressTest.xaml", UriKind.Relative));
		}

		private static void AdjustSystemTray(bool isVisible = true, double opacity = 1)
		{
			SystemTray.IsVisible = isVisible;
			SystemTray.Opacity = opacity;
		}
     }
}