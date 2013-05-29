using Coding4Fun.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DemoApp__WinStore_
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		public MainPage()
        {
            this.InitializeComponent();


			var targetFile = "robot.jpg";
			var targetFileInAppData = "robotInAppData.jpg";

			string contentFolderName = Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Images");

			StorageFolder contentFolder = StorageFolder.GetFolderFromPathAsync(contentFolderName).AsTask().Result;
			StorageFile contentFile = contentFolder.GetFileAsync(targetFile).AsTask().Result;

			try
			{
				var item = contentFile.CopyAsync(ApplicationData.Current.LocalFolder, targetFileInAppData).AsTask().Result;
			}
			catch (Exception)
			{
			}
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override  void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}