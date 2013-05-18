using System;
using System.Collections.Generic;
using System.IO;
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

		    var targetFile = "robot.jpg";
		    // Sample code to localize the ApplicationBar
		    //BuildLocalizedApplicationBar();

			using (var stream = PlatformFileAccess.GetSaveFileStream(targetFile)) // saving into iso
		    {
				using (var trexOrginial = File.Open("assets/images/" + targetFile, FileMode.Open))
			    {
				    var trexBytes = new byte[trexOrginial.Length];

				    trexOrginial.Write(trexBytes, 0, (int) trexOrginial.Length);

				    stream.Write(trexBytes, 0, trexBytes.Count());
			    }
		    }

			using (var stream = PlatformFileAccess.GetOpenFileStream(targetFile)) // saving into iso
			{
				//var bitmap = PictureDecoder.DecodeJpeg(stream);
				//img.SetSource(stream);
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