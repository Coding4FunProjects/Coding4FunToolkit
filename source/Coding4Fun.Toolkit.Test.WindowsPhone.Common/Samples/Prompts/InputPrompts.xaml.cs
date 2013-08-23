using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Coding4Fun.Toolkit.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Prompts
{
	public partial class InputPrompts : PhoneApplicationPage
	{
		private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 240, 248, 255));
		private readonly SolidColorBrush _naturalBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 135, 189));
		private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 100, 149, 237));

		const string LongText = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";

		public InputPrompts()
		{
			InitializeComponent();
		}

		#region input prompt
		private void InputClick(object sender, RoutedEventArgs e)
		{
			var input = new InputPrompt
			{
				Title = "Basic Input",
				Message = "I'm a basic input prompt" + LongText,
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

		private void InputLongMsgClick(object sender, RoutedEventArgs e)
		{
			var input = new InputPrompt
			{
				Title = "Basic Input",
				Message = LongText,
				MessageTextWrapping = TextWrapping.Wrap
			};

			input.Completed += PopUpPromptStringCompleted;

			input.Show();
		}
		
		#endregion

		void PopUpPromptStringCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			Results.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
		}

		private void DingClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
		}
	}
}