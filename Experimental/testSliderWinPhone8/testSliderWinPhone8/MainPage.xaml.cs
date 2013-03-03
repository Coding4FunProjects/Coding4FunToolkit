using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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
		}

		private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var slide = sender as Slider;

			if (slide == null)
				return;

			if (slide.Value % slide.LargeChange < slide.LargeChange / 2.0)
			{
				slide.Value = Math.Floor((slide.Value / slide.LargeChange)) * slide.LargeChange;
			}
			else
			{
				slide.Value = Math.Ceiling((slide.Value / slide.LargeChange)) * slide.LargeChange;
			}	
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