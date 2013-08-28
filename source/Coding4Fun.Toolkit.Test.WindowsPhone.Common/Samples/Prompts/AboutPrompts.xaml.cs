using System.Windows;

using Coding4Fun.Toolkit.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Prompts
{
	public partial class AboutPrompts : PhoneApplicationPage
	{
		private AboutPrompt _prompt;

		public AboutPrompts()
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
				_prompt = new AboutPrompt();
			}

			_prompt.Completed += PromptCompleted;
		}

		private void AboutPromptBlankClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Show();
		}

		private void AboutPromptBasicClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Show("Clint Rutkas", "ClintRutkas", "Clint@Rutkas.com", "http://betterthaneveryone.com");
		}

		private void AboutPromptLongClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Title = "Custom Title";
			_prompt.VersionNumber = "v3.14159265";

			_prompt.Show(
				new AboutPromptItem { Role = "dev", AuthorName = "Clint Rutkas" },
				new AboutPromptItem { Role = "site", WebSiteUrl = "http://coding4fun.com" });
		}

		private void AboutPromptC4FClick(object sender, RoutedEventArgs e)
		{
			InitializePrompt();

			_prompt.Show("Clint Rutkas", "ClintRutkas", "Clint@Rutkas.com", "http://betterthaneveryone.com");
		}

		void PromptCompleted(object sender, PopUpEventArgs<object, PopUpResult> e)
		{
			Results.Text = e.PopUpResult.ToString();
		}

		private void DingClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
		}
	}
}