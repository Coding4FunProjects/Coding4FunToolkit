using System;
using System.Globalization;

namespace Coding4Fun.Toolkit.Controls.Converters
{
    public class VisibilityToBooleanConverter : ValueConverter
    {
        private static readonly BooleanToVisibilityConverter BoolToVis = new BooleanToVisibilityConverter();

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            return BoolToVis.ConvertBack(value, targetType, parameter, culture);
        }

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            return BoolToVis.Convert(value, targetType, parameter, culture);
        }
    }
}