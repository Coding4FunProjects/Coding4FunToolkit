using System.Windows;

using Coding4Fun.Phone.Controls;
using Coding4Fun.Phone.Site.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.Prompts
{
	public partial class AboutPrompts : PhoneApplicationPage
	{
		public AboutPrompts()
		{
			InitializeComponent();
		}

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
				new AboutPersonItem { Role = "dev", AuthorName = "Clint Rutkas" },
				new AboutPersonItem { Role = "site", WebSiteUrl = "http://coding4fun.com" });
		}

		private void AboutPromptC4FClick(object sender, RoutedEventArgs e)
		{
			var about = new Coding4FunAboutPrompt();

			about.Show("Clint Rutkas", "ClintRutkas", "Clint@Rutkas.com", "http://betterthaneveryone.com");
		}
		#endregion

		void PopUpPromptObjectCompleted(object sender, PopUpEventArgs<object, PopUpResult> e)
		{
			resultBlock.Text = e.PopUpResult.ToString();
		}

		private void DingClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
		}
	}
}