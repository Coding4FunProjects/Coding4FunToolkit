using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Coding4Fun.Phone.Controls.Converters
{
    public class VisibilityToBooleanConverter : IValueConverter
    {
        private readonly BooleanToVisibilityConverter _boolToVis = new BooleanToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _boolToVis.ConvertBack(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _boolToVis.Convert(value, targetType, parameter, culture);
        }
    }
}
