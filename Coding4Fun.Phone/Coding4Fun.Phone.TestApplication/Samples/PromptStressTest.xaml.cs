using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Coding4Fun.Phone.Controls;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Coding4Fun.Phone.TestApplication.Samples
{
	public partial class PromptStressTest : PhoneApplicationPage
	{
		public PromptStressTest()
		{
			InitializeComponent();
		}

		private void back_Click(object sender, RoutedEventArgs e)
		{
			var messagePrompt = new MessagePrompt
			{
				Title = "Advanced Message",
				Message = "When complete, i'll navigate back",
			};

			messagePrompt.Completed += navBack_Completed;

			messagePrompt.Show();
		}

		private void newPage_Click(object sender, RoutedEventArgs e)
		{
			var messagePrompt = new MessagePrompt
			{
				Title = "Advanced Message",
				IsCancelVisible = true,
				Body = "Nav to Color page page on complete"
			};

			messagePrompt.Completed += navTo_Completed;

			messagePrompt.Show();
		}

		private void newPageViaBodyButtn_Click(object sender, RoutedEventArgs e)
		{
			var messagePrompt = new MessagePrompt
			{
				Title = "Advanced Message",
				IsCancelVisible = true,
			};

			var btn = new Button { Content = "Nav to Colors" };
			btn.Click += (s, ee) => NavigationService.Navigate(new Uri("/Samples/ColorControls.xaml", UriKind.Relative));
			messagePrompt.Body = btn;

			messagePrompt.Completed += string_Completed;

			messagePrompt.Show();
		}

		void navBack_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			NavigationService.GoBack();
		}

		void navTo_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			NavigationService.Navigate(new Uri("/Samples/ColorControls.xaml", UriKind.Relative));
		}

		private void Toast_Click(object sender, RoutedEventArgs e)
		{
			var toast = new ToastPrompt { Title = " Basic", Message = "Message" };
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
				Title = "Advanced",
				Message = "Custom Fontsize, img, and orientation",
				FontSize = 50,
				TextOrientation = System.Windows.Controls.Orientation.Vertical,
				ImageSource =
					new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute))
			};

			toast.Completed += string_Completed;
			toast.Show();
		}

		void string_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			MessageBox.Show(e.PopUpResult.ToString());
		}

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var toast = new ToastPrompt { Message = "attempt to get onnavigateto failure" };
            toast.Show();

            base.OnNavigatedTo(e);
        }

        private void AsyncToastWithNav_Click(object sender, RoutedEventArgs e)
        {
            //var t = new Thread(ToastCall);
            ToastCall();
            NavigationService.GoBack();
            
            //t.Start();
        }

        private void ToastCall()
        {
             Dispatcher.BeginInvoke(() =>
                                       {
                                           var toast = new ToastPrompt { Message = "Hi from the past" };
                                           toast.Show();
                                       });
        }

        private void LargeToastCreation_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((state) => 
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(33);
                    Dispatcher.BeginInvoke(() =>
                                               {
                                                   var toast = new ToastPrompt { Message = i.ToString() };
                                                   toast.Show();
                                               });
                }
            });
        }

		private void PromptVis_Click(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray();
			CreateMsgPrompt("Test Vis");
		}

		private void PromptNotVis_Click(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray(false);
			CreateMsgPrompt("Test not Vis");
		}

		private void PromptVisWithOpacity_Click(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray(true, .8);
			CreateMsgPrompt("Test with Opacity");
		}
		
		private static void AdjustSystemTray(bool isVisible = true, double opacity = 1)
		{
			SystemTray.IsVisible = isVisible;
			SystemTray.Opacity = opacity;
		}

		private static void CreateMsgPrompt(string message)
		{
			var msgPrompt = new MessagePrompt { Title = message, Message = message };
			msgPrompt.Show();
		}
	}
}