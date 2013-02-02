using System;
using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
	public partial class ChatBubbleControls : PhoneApplicationPage
	{
		public ChatBubbleControls()
		{
			InitializeComponent();
		}

		private void ChatBubbleClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("/Samples/ChatBubbles/ChatBubbles.xaml");
		}

		private void ChatBubbleTextBoxClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("/Samples/ChatBubbles/ChatBubbleTextBoxes.xaml");
		}

		private void NavigateTo(string page)
		{
			NavigationService.Navigate(new Uri(page, UriKind.Relative));
		}
	}
}