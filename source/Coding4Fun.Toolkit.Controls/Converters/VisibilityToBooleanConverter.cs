using System;
using System.Globalization;
using System.Windows.Data;


namespace Coding4Fun.Toolkit.Controls.Converters
{
    public class VisibilityToBooleanConverter : IValueConverter
    {
        private static readonly BooleanToVisibilityConverter BoolToVis = new BooleanToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BoolToVis.ConvertBack(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BoolToVis.Convert(value, targetType, parameter, culture);
        }
    }
}
