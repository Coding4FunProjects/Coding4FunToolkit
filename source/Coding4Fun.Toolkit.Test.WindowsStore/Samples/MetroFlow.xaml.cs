using Windows.UI.Xaml.Input;

using Coding4Fun.Toolkit.Test.WindowsStore.Samples.Buttons;
using System;
using Windows.UI.Xaml.Media.Imaging;
using Coding4Fun.Toolkit.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class MetroFlow
	{
        public MetroFlow()
		{
			InitializeComponent();

            loadImageAtRunTime.ImageSource = new BitmapImage(new Uri(@"ms-appx:/Media/images/trex_360width.jpg", UriKind.RelativeOrAbsolute));

            loadDataAtRunTime.Items.Add(new MetroFlowData { Title = "Test 1", ImageUri = new Uri(@"ms-appx:/Media/images/Robot.jpg", UriKind.RelativeOrAbsolute) });
            loadDataAtRunTime.Items.Add(new MetroFlowData { Title = "Test 2", ImageUri = new Uri(@"ms-appx:/Media/images/trex_360width.jpg", UriKind.RelativeOrAbsolute) });
            loadDataAtRunTime.Items.Add(new MetroFlowData { Title = "Test 3", ImageUri = new Uri(@"ms-appx:/Media/images/lamp.jpg", UriKind.RelativeOrAbsolute) });
            loadDataAtRunTime.Items.Add(new MetroFlowData
            {
                Title = "coding4fun",
                ImageUri = new Uri(@"ms-appx:/Media/headwhite_100.png", UriKind.RelativeOrAbsolute)
            });

            loadDataAtRunTime.Items.Add(new MetroFlowData
            {
                Title = "Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet",
                ImageUri = new Uri(@"ms-appx:/Media/images/Seattle.jpg", UriKind.RelativeOrAbsolute)
            });
		}

		
	}
}
