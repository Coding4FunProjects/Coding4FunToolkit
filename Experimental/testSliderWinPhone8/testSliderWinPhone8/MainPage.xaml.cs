using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using testSliderWinPhone8.Resources;

namespace testSliderWinPhone8
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			InitializeComponent();

			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();

			forceSet.Thumb = new Rectangle {Fill = new SolidColorBrush(Colors.Green), Width = 24, Height = 24};
		}

		private void ColorClick(object sender, RoutedEventArgs e)
		{
			NavigationHelper.NavigateTo("ColorSliders.xaml");
		}

		private void VerticalClick(object sender, RoutedEventArgs e)
		{
			NavigationHelper.NavigateTo("VerticalSliders.xaml");
		}

		private void HorizontalClick(object sender, RoutedEventArgs e)
		{
			NavigationHelper.NavigateTo("MainPage.xaml");
		}

		// Sample code for building a localized ApplicationBar
		//private void BuildLocalizedApplicationBar()
		//{
		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.
		//    ApplicationBar = new ApplicationBar();

		//    // Create a new button and set the text value to the localized string from AppResources.
		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
		//    appBarButton.Text = AppResources.AppBarButtonText;
		//    ApplicationBar.Buttons.Add(appBarButton);

		//    // Create a new menu item with the localized string from AppResources.
		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
		//    ApplicationBar.MenuItems.Add(appBarMenuItem);
		//}
	}
}