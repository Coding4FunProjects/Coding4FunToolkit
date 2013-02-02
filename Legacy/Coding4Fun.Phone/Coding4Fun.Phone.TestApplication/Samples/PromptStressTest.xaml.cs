using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using Coding4Fun.Phone.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
	public partial class PromptStressTest : PhoneApplicationPage
	{
		public PromptStressTest()
		{
			InitializeComponent();
		}

		#region back nav test
		private void back_Click(object sender, RoutedEventArgs e)
		{
			var messagePrompt = new MessagePrompt
			{
				Title = "Advanced Message",
				Message = "When prompt is complete, i'll navigate back",
			};

			messagePrompt.Completed += navBack_Completed;

			messagePrompt.Show();
		}

		void navBack_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			NavigationService.GoBack();
		}
		#endregion
		#region nav to test
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

		void navTo_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			NavigationService.Navigate(new Uri("/Samples/ColorControls.xaml", UriKind.Relative));
		}
		#endregion
		#region nav to test within own msg
		private void NavToNewPageViaBodyButtonClick(object sender, RoutedEventArgs e)
		{
			var messagePrompt = new MessagePrompt
			{
				Title = "Advanced Message",
				IsCancelVisible = true,
			};

			var btn = new Button { Content = "Nav to Colors" };
			btn.Click += (s, ee) => NavigationService.Navigate(new Uri("/Samples/ColorControls.xaml", UriKind.Relative));
			messagePrompt.Body = btn;

			messagePrompt.Show();
		}
		#endregion
		
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
			var toast = new ToastPrompt { Message = "attempt to get OnNavigatedTo failure" };
            toast.Show();

            base.OnNavigatedTo(e);
        }

        private void AsyncToastWithNav_Click(object sender, RoutedEventArgs e)
        {
			Dispatcher.BeginInvoke(() =>
			{
				var toast = new ToastPrompt { Message = "Hi from the past" };
				toast.Show();
			});

            NavigationService.GoBack();
        }

        private void MassToastCreationClick(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(state => 
            {
                for (var i = 0; i < 100; i++)
                {
                    Thread.Sleep(10);
                    Dispatcher.BeginInvoke(() =>
                                               {
                                                   var toast = new ToastPrompt { Message = i.ToString() };
                                                   toast.Show();
                                               });
                }
            });
        }
	}
}