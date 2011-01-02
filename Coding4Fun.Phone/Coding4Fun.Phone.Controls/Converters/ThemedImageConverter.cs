using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Collections.Generic;

namespace Coding4Fun.Phone.Controls.Converters
{
	public class ThemedImageConverter : IValueConverter
	{
		private static readonly Dictionary<String, BitmapImage> ImageCache = new Dictionary<string, BitmapImage>();

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var isDarkTheme = (Application.Current.Resources.Contains("PhoneDarkThemeVisibility") && ((Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"]) == Visibility.Visible);
			BitmapImage result = null;
			
			var str = value as string;
			// Path to the icon image
			if (str != null)
			{
				// Path to the icon image
				var path = string.Format(str, isDarkTheme ? "dark" : "light");

				// Check if we already cached the image
				if (!ImageCache.TryGetValue(path, out result))
				{
					result = new BitmapImage(new Uri(path, UriKind.Relative));
					ImageCache.Add(path, result);
				}
			}
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
