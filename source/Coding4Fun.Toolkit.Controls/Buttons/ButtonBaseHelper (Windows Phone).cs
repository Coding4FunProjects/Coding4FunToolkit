using System.Linq;

using Coding4Fun.Toolkit.Controls.Common;

#if WINDOWS_STORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

			for (int i = imgRects.Count() - 1; i >= 0; i--)
			{
				hostBody.Children.Remove(imgRects[i]);
			}

			if (imageSource == null)
				return;

			var imgBrush = new ImageBrush { Opacity = 0, ImageSource = imageSource, Stretch = stretch };
			var imgRect = new Rectangle
			{
				OpacityMask = imgBrush
			};

			hostBody.Children.Add(imgRect);

			ApplyForegroundToFillBinding(content, imgRect);

			var sb = new Storyboard();
			ControlHelper.CreateDoubleAnimations(sb, imgBrush, "Opacity", 0.2, 1, 75);
			hostBody.Dispatcher.BeginInvoke(sb.Begin);
		}
	}
}