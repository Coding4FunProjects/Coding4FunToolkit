using System;

#if WINDOWS_STORE
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

#elif WINDOWS_PHONE
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#endif

namespace Coding4Fun.Toolkit.Controls
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

		public static void ApplyOpacityImageBrush(FrameworkElement item, ImageBrush brush, DependencyProperty imageDependencyProperty)
		{
			var image = item.GetValue(imageDependencyProperty) as ImageSource;

			if (image == null)
				item.SetValue(imageDependencyProperty, new BitmapImage(new Uri("/Coding4Fun.Toolkit.Controls;component/Media/appbar.check.rest.png", UriKind.RelativeOrAbsolute)));
			else 
				SetImageBrush(brush, image);
		}

		public static void ApplyStretch(ImageBrush brush, Stretch stretch)
		{
			SetStretch(brush, stretch);
		}

		public static void ApplyTitleOffset(ContentControl contentTitle)
		{
			if (contentTitle == null) 
				return;

			var bottom = -(contentTitle.FontSize / 8.0);
			var top = -(contentTitle.FontSize / 2.0) - bottom;

			contentTitle.Margin = new Thickness(0, top, 0, bottom);
		}

		public static void ApplyForegroundToFillBinding(ContentControl control)
		{
			if (control == null)
				return;

			var element = control.Content as FrameworkElement;

			if (element == null) 
				return;

			if (element.GetType() == typeof(Shape))
			{
				ApplyForegroundToFillBinding(control, element);
			}
			else
			{
				var children = element.GetLogicalChildrenByType<Shape>(false).Where(child => child.Fill == null);

				foreach (var child in children)
				{
					ApplyForegroundToFillBinding(control, child);
				}
			}
		}

		public static void ApplyForegroundToFillBinding(FrameworkElement source, FrameworkElement target)
		{
			ApplyBinding(source, target, "Foreground", Shape.FillProperty);
		}

		private static void ApplyBinding(FrameworkElement source, FrameworkElement target, string propertyPath, DependencyProperty property)
		{
#if WINDOWS_STORE
			var binding = new Windows.UI.Xaml.Data.Binding();
#elif WINDOWS_PHONE
			var binding = new System.Windows.Data.Binding();;
#endif

			binding.Source = source;
			binding.Path = new PropertyPath(propertyPath);

			target.SetBinding(property, binding);
		}
	}
}
