using System;

#if WINDOWS_PHONE
using System.Globalization;
using System.Windows;
using System.Windows.Data;
#elif WINDOWS_STORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace Coding4Fun.Toolkit.Controls
{
    public class StringVisibilityConverter : IValueConverter
    {
#if WINDOWS_STORE
        public object Convert(object value, Type targetType, object parameter, string language)
#elif WINDOWS_PHONE
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
        {
            return string.IsNullOrEmpty(value.ToString()) ? Visibility.Collapsed : Visibility.Visible;
        }

#if WINDOWS_STORE
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#elif WINDOWS_PHONE
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
        {
            throw new System.NotImplementedException();
        }
    }

    public class VisibilityConverter: IValueConverter
    {
        public bool Inverse { get; set; }

#if WINDOWS_STORE
        public object Convert(object value, Type targetType, object parameter, string language)
#elif WINDOWS_PHONE
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
        {
            if (value == null) return Visibility.Collapsed;

            if (Inverse)
            {
                return (bool) value ? Visibility.Collapsed : Visibility.Visible;
            }

            return (bool) value ? Visibility.Visible : Visibility.Collapsed;
        }

#if WINDOWS_STORE
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#elif WINDOWS_PHONE
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
        {
            throw new NotImplementedException();
        }
    }
}
