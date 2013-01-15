using System.Linq;

#if WINDOWS_STORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	internal static partial class ButtonBaseHelper
	{
		public static void UpdateImageSource(FrameworkElement content, Grid hostBody, ImageSource imageSource, Stretch stretch)
		{
			if (hostBody == null || content == null)
				return;

			var imgRects = hostBody.Children.OfType<Rectangle>().ToArray();

			for (var i = 0; i < imgRects.Count(); i++)
				hostBody.Children.Remove(imgRects[0]);

			if (imageSource == null)
				return;

			var imgRect = new Rectangle
				{
					OpacityMask = new ImageBrush {ImageSource = imageSource, Stretch = stretch},
				};

			ApplyForegroundToFillBinding(content, imgRect);

			hostBody.Children.Add(imgRect);
		}
	}
}