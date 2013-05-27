using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

			var currentLocation = "assets/images/" + targetFile;

			// TODO: INSERT FILE

			//var storageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

			//var file = await storageFolder.GetFileAsync(targetFile);

			
			//await checkedImageSource.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read)

			//using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
			//{
			//	if (storage.FileExists(targetFile)) return;

			//	var sri = Application.GetResourceStream(new Uri(currentLocation, UriKind.Relative));

			//	if (sri != null)
			//	{
			//		using (var stream = storage.CreateFile(targetFile))
			//		{
			//			const int chunkSize = 4096;
			//			var bytes = new byte[chunkSize];
			//			int byteCount;

			//			while ((byteCount = sri.Stream.Read(bytes, 0, chunkSize)) > 0)
			//			{
			//				stream.Write(bytes, 0, byteCount);
			//			}
			//		}
			//	}
			//}
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
