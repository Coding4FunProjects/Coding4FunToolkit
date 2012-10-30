using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Coding4Fun.Toolkit.Controls;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Prompts
{
	public partial class MessagePrompts : PhoneApplicationPage
	{
		private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 240, 248, 255));
		private readonly SolidColorBrush _naturalBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 135, 189));
		private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 100, 149, 237));

		const string LongText = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";


		public MessagePrompts()
		{
			InitializeComponent();
		}

		#region message prompt
		private void MessageClick(object sender, RoutedEventArgs e)
		{
			var messagePrompt = new MessagePrompt
			{
				Title = "Basic Message",
				Message = LongText,
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

		void PopUpPromptStringCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			resultBlock.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
		}

		private void DingClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
		}

		private static void AdjustSystemTray(bool isVisible = true, double opacity = 1)
		{
			SystemTray.IsVisible = isVisible;
			SystemTray.Opacity = opacity;
		}
	}
}