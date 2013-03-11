using Windows.UI.Xaml;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples.ChatBubbles
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class ChatBubbleTextBoxTests
	{
		public ChatBubbleTextBoxTests()
		{
			InitializeComponent();
		}

		private void AddTextClicked(object sender, RoutedEventArgs e)
		{
			_dynamicTextHintTest.Text += "c4f ";
		}
	}
}