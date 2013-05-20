using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Storage;
using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DemoApp.Resources;

namespace DemoApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
	    public MainPage()
	    {
		    InitializeComponent();

		    // Sample code to localize the ApplicationBar
		    //BuildLocalizedApplicationBar();
			var targetFile = "robot.jpg";

			var currentLocation = "assets/images/" + targetFile;

			using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (storage.FileExists(targetFile)) return;

				var sri = Application.GetResourceStream(new Uri(currentLocation, UriKind.Relative));

				if (sri != null)
				{
					using (var stream = storage.CreateFile(targetFile))
					{
						const int chunkSize = 4096;
						var bytes = new byte[chunkSize];
						int byteCount;

						while ((byteCount = sri.Stream.Read(bytes, 0, chunkSize)) > 0)
						{
							stream.Write(bytes, 0, byteCount);
						}
					}
				}
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