using System;
using System.Globalization;

#if WINDOWS_STORE

using Windows.UI.Xaml;

#elif WINDOWS_PHONE

using System.Windows;

#endif

namespace Coding4Fun.Toolkit.Controls.Converters
{
	public class StringToVisibilityConverter : ValueConverter
    {
	    public bool Inverted { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            if (Inverted)
            {
                return string.IsNullOrEmpty(value as string) ? Visibility.Visible : Visibility.Collapsed;
            }

			return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
        }

	    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            throw new NotImplementedException();
        }
    }
}