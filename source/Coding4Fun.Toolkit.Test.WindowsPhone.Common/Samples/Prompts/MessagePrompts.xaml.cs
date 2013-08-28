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

		private MessagePrompt _prompt;

		public MessagePrompts()
		{
			InitializeComponent();
		}

		private void InitializePrompt()
		{
			//var reuseObject = ReuseObject.IsChecked.GetValueOrDefault(false);

			if (_prompt != null)
			{
				_prompt.Completed -= PromptCompleted;
			}

			//if (!reuseObject || _prompt == null)
			{
				_prompt = new MessagePrompt();
			}

			_prompt.Completed += PromptCompleted;
		}

		private void MessageClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Title = "Basic Message";
			_prompt.Message = LongText;

			_prompt.Show();
		}

		private void MessageAdvancedClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Title = "Advanced Message";
			_prompt.Message = "When complete, i'll navigate back";
			_prompt.Overlay = _cornFlowerBlueSolidColorBrush;
			_prompt.IsCancelVisible = true;

			_prompt.Show();
		}

		private void MessageCustomClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Title = "Custom Body Message";
			_prompt.Background = _naturalBlueSolidColorBrush;
			_prompt.Foreground = _aliceBlueSolidColorBrush;
			_prompt.Overlay = _cornFlowerBlueSolidColorBrush;
			_prompt.IsCancelVisible = true;

			var btn = new Button { Content = "Msg Box" };
			btn.Click += (s, args) => Results.Text = "Hi!";

			_prompt.Body = btn;

			_prompt.Show();
		}

		private void MessageSuperClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Title = "Advanced Message";
			_prompt.Background = _naturalBlueSolidColorBrush;
			_prompt.Foreground = _aliceBlueSolidColorBrush;
			_prompt.Overlay = _cornFlowerBlueSolidColorBrush;

			var btnHide = new RoundButton { Label = "Hide" };
			btnHide.Click += (o, args) => _prompt.Hide();

			var btnComplete = new RoundButton { Label = "Complete" };
			btnComplete.Click += (o, args) => _prompt.OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.Ok, Result = "You clicked the Complete Button" });

			_prompt.ActionPopUpButtons.Clear();
			_prompt.ActionPopUpButtons.Add(btnHide);
			_prompt.ActionPopUpButtons.Add(btnComplete);

			_prompt.Show();
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

		void PromptCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			Results.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
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