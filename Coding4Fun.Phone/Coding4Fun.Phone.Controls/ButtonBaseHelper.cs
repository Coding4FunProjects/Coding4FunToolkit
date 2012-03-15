using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Coding4Fun.Phone.Controls
{
	internal static class ButtonBaseHelper
	{
		public static void OnImageChange(DependencyPropertyChangedEventArgs e, ImageBrush brush)
		{ 
            if (e.NewValue == e.OldValue)
                return;

            SetImageBrush(brush, e.NewValue as ImageSource);
		}

		public static void SetImageBrush(ImageBrush brush, ImageSource imageSource)
		{
			if (brush == null)
				return;

			brush.ImageSource = imageSource;
		}

		public static void OnStretch(DependencyPropertyChangedEventArgs e, ImageBrush brush)
		{
			if (e.NewValue == e.OldValue)
				return;

			SetStretch(brush, (Stretch)e.NewValue);
		}

		public static void SetStretch(ImageBrush brush, Stretch stretch)
		{
			if (brush == null)
				return;

			brush.Stretch = stretch;
		}

		public static void ApplyTemplate(FrameworkElement item, ImageBrush brush, ContentControl contentBody, Stretch stretch, DependencyProperty imageDependencyProperty)
		{
			SetStretch(brush, stretch);

			if (contentBody != null)
			{
				var bottom = -(contentBody.FontSize / 8.0);
				var top = -(contentBody.FontSize / 2.0) - bottom;

				contentBody.Margin = new Thickness(0, top, 0, bottom);
			}
			
			var image = item.GetValue(imageDependencyProperty) as ImageSource;

			if (image == null)
				item.SetValue(imageDependencyProperty, new BitmapImage(new Uri("/Coding4Fun.Phone.Controls;component/Media/icons/appbar.check.rest.png", UriKind.RelativeOrAbsolute)));
			else 
				SetImageBrush(brush, image);
		}
	}
}
