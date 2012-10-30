using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Coding4Fun.Toolkit.Controls.Converters
{
	public static class ThemedImageConverterHelper
	{
		private static readonly Dictionary<string, BitmapImage> ImageCache = new Dictionary<string, BitmapImage>();

		public static BitmapImage GetImage(string path, bool negateResult = false)
		{
            if (string.IsNullOrEmpty(path))
                return null;

			var isDarkTheme = (Application.Current.Resources.Contains("PhoneDarkThemeVisibility") &&
			                   ((Visibility) Application.Current.Resources["PhoneDarkThemeVisibility"]) == Visibility.Visible);

			if (negateResult)
				isDarkTheme = !isDarkTheme;

			BitmapImage result;
			path = string.Format(path, isDarkTheme ? "dark" : "light");

			// Check if we already cached the image
			if (!ImageCache.TryGetValue(path, out result))
			{
				result = new BitmapImage(new Uri(path, UriKind.Relative));
				ImageCache.Add(path, result);
			}

			return result;
		}
	}
}
