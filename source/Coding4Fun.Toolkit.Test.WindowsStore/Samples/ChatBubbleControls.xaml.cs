using Windows.UI.Xaml.Input;

using Coding4Fun.Toolkit.Test.WindowsStore.Samples.ChatBubbles;


namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class ChatBubbleControls
	{
		public ChatBubbleControls()
		{
			InitializeComponent();
		}

		private void ChatBubbleControlsTapped(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(ChatBubbleTests));
		}

		private void ChatBubbleTextBoxControlsTapped(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(ChatBubbleTextBoxTests));
		}
	}
}
