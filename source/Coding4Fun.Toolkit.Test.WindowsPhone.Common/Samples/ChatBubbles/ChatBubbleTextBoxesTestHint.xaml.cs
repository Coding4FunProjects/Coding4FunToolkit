using System.Windows.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.ChatBubbles
{
	public partial class ChatBubbleTextBoxesTestHint : UserControl
	{
		public ChatBubbleTextBoxesTestHint()
		{
			InitializeComponent();
		}

		private void AddTextClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			DynamicTextHintTest.Text += "c4f ";
		}
	}
}
