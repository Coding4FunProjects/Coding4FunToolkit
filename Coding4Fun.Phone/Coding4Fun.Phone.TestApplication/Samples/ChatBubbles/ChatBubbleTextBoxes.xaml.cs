using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.ChatBubbles
{
	public partial class ChatBubbleTextBoxes : PhoneApplicationPage
	{
		public ChatBubbleTextBoxes()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DynamicTextHintTest.Text += "c4f ";
		}
	}
}