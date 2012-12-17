#if WINDOWS_STORE

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE

using System.Windows;
using System.Windows.Media;

#endif

namespace Coding4Fun.Toolkit.Controls.Binding
{
	public class FrameworkElementBinding
	{
		#region ClipToBounds
		#region DependencyProperty
		public static bool GetClipToBounds(DependencyObject obj)
		{
			return (bool)obj.GetValue(ClipToBoundsProperty);
		}

		public static void SetClipToBounds(DependencyObject obj, bool value)
		{
			obj.SetValue(ClipToBoundsProperty, value);
		}

		public static readonly DependencyProperty ClipToBoundsProperty =
			DependencyProperty.RegisterAttached(
				"ClipToBounds",
				typeof(bool),
				typeof(FrameworkElementBinding),
				new PropertyMetadata(false, OnClipToBoundsPropertyChanged));
		#endregion

		private static void OnClipToBoundsPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == e.OldValue)
				return;

			HandleClipToBoundsEventAppend(obj, (bool)e.NewValue);
		}

		private static void HandleClipToBoundsEventAppend(object sender, bool value)
		{
			var item = sender as FrameworkElement;

			if (item == null)
				return;

			SetClippingBound(item, value);

			if (value)
			{
				item.Loaded += ClipToBoundsPropertyChanged;
				item.SizeChanged += ClipToBoundsPropertyChanged;
			}
			else
			{
				item.Loaded -= ClipToBoundsPropertyChanged;
				item.SizeChanged -= ClipToBoundsPropertyChanged;
			}
		}

		private static void ClipToBoundsPropertyChanged(object sender, RoutedEventArgs e)
		{
			var item = sender as FrameworkElement;

			if (item == null)
				return;

			SetClippingBound(item, GetClipToBounds(item));
		}

		private static void SetClippingBound(FrameworkElement element, bool setClippingBound)
		{
			if (setClippingBound)
			{
				element.Clip =
					new RectangleGeometry
					{
						Rect = new Rect(0, 0, element.ActualWidth, element.ActualHeight)
					};
			}
			else
			{
				element.Clip = null;
			}
		}
		#endregion
	}
}