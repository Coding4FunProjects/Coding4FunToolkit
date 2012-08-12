using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class ButtonControls : PhoneApplicationPage
    {
        public ButtonControls()
        {
            InitializeComponent();
        }

		private void NavToTilesClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("Tiles.xaml");
		}

		private void NavToImageTilesClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("ImageTiles.xaml");
		}

		private void NavToRoundButtonsClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("RoundButtons.xaml");
		}

		private void NavToRoundToggleButtonsClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("RoundToggleButtons.xaml");
		}

		private void NavToOpacityToggleButtonsClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("OpacityToggleButtons.xaml");
		}

		private void NavToStressClick(object sender, RoutedEventArgs e)
		{
			NavigateTo("StressTests.xaml");
		}

		private void NavigateTo(string page)
		{
			NavigationService.Navigate(new Uri("/Samples/Buttons/" + page, UriKind.Relative));
		}		
    }
}