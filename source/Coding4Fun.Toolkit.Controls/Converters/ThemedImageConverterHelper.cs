using System;
using System.Collections.Generic;

#if WINDOWS_STORE || WINDOWS_PHONE_APP

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

#elif WINDOWS_PHONE

using System.Windows;
using System.Windows.Media.Imaging;

#endif

namespace Coding4Fun.Toolkit.Controls.Converters
{
	public static class ThemedImageConverterHelper
	{
		private static readonly Dictionary<string, BitmapImage> ImageCache = new Dictionary<string, BitmapImage>();

		public static BitmapImage GetImage(string path, bool negateResult = false)
		{
            if (string.IsNullOrEmpty(path))
                return null;

			var isDarkTheme =
#if WINDOWS_STORE || WINDOWS_PHONE_APP
 Application.Current.RequestedTheme == ApplicationTheme.Dark;
#elif WINDOWS_PHONE
				(Application.Current.Resources.Contains("PhoneDarkThemeVisibility") &&
			                   ((Visibility) Application.Current.Resources["PhoneDarkThemeVisibility"]) == Visibility.Visible);
#endif

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