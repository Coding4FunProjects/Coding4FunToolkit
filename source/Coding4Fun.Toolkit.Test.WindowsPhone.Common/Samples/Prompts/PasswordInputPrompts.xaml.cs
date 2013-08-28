using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Coding4Fun.Toolkit.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Prompts
{
	public partial class PasswordInputPrompts : PhoneApplicationPage
	{
		private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 240, 248, 255));
		private readonly SolidColorBrush _naturalBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 135, 189));
		private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 100, 149, 237));

		const string LongText = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";

		private PasswordInputPrompt _prompt;

		public PasswordInputPrompts()
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
				_prompt = new PasswordInputPrompt();
			}

			_prompt.Completed += PromptCompleted;
		}

		private void PasswordClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Title = "Basic Input";
			_prompt.Message = "I'm a basic input prompt" + LongText;

			_prompt.Show();
		}

		private void PasswordNoEnterClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Title = "Enter won't submit";
			_prompt.Message = "Enter key won't submit now";
			_prompt.IsSubmitOnEnterKey = false;

			_prompt.Show();
		}

		private void PasswordAdvancedClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Title = "TelephoneNum";
			_prompt.Message = "I'm a message about Telephone numbers!";
			_prompt.Background = _naturalBlueSolidColorBrush;
			_prompt.Foreground = _aliceBlueSolidColorBrush;
			_prompt.Overlay = _cornFlowerBlueSolidColorBrush;
			_prompt.IsCancelVisible = true;
			_prompt.InputScope = new InputScope {Names = {new InputScopeName {NameValue = InputScopeNameValue.TelephoneNumber}}};
			_prompt.Value = "Value Return";

			_prompt.Show();
		}

		private void InputLongMsgClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Title = "Basic Input";
			_prompt.Message = LongText;
			_prompt.MessageTextWrapping = TextWrapping.Wrap;
			
			_prompt.Show();
		}

		void PromptCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			Results.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
		}

		private void DingClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
		}
	}
}