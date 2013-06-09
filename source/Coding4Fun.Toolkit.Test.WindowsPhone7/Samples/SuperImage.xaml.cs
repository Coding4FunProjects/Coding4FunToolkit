using System;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
    public partial class SuperImage : PhoneApplicationPage
    {
        // Constructor
	    public SuperImage()
	    {
		    InitializeComponent();

		    // Sample code to localize the ApplicationBar
		    //BuildLocalizedApplicationBar();
			var targetFile = "robot.jpg";
			var targetFileIso = "robotIso.jpg";

			var currentLocation = "./Media/images/" + targetFile;

			using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (storage.FileExists(targetFileIso)) return;

				var sri = Application.GetResourceStream(new Uri(currentLocation, UriKind.Relative));

				if (sri != null)
				{
					using (var stream = storage.CreateFile(targetFileIso))
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
    }
}