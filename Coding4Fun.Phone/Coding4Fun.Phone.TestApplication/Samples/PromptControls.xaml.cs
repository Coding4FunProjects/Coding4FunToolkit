using System;
using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class PromptControls : PhoneApplicationPage
    {
		public PromptControls()
        {
            InitializeComponent();
        }

		private void Ding_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
        }

		private void NavToToastPromptsClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("ToastPrompts.xaml");
		}

		private void NavToMessagePromptsClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("MessagePrompts.xaml");
		}

		private void NavToInputPromptsClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("InputPrompts.xaml");
		}
		
		private void NavToPasswordInputPromptsClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("PasswordInputPrompts.xaml");
		}

		private void NavToAboutPromptsClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("AboutPrompts.xaml");
		}
		
		private void NavigateTo(string page)
		{
			NavigationService.Navigate(new Uri("/Samples/Prompts/" + page, UriKind.Relative));
		}

        private void NavToAppBarPromptsClick(object sender, RoutedEventArgs e)
        {
            NavigateTo("AppBarPrompts.xaml");
        }

     }
}