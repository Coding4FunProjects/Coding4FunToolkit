using System;
using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Buttons
{
	public partial class StressTests : PhoneApplicationPage
	{
		public StressTests()
		{
			InitializeComponent();
		}

		private void NavAway_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
		}
	}
}