using System;
using System.Globalization;
using System.Windows.Data;

namespace Coding4Fun.Toolkit.Controls.Converters
{
    public class ThemedInverseImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formatString = parameter as string;

            if (string.IsNullOrEmpty(formatString))
                formatString = value as string;

            return ThemedImageConverterHelper.GetImage(formatString, true);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
