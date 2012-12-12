using System;
using System.Globalization;

namespace Coding4Fun.Toolkit.Controls.Converters
{
    public class ThemedInverseImageConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            var formatString = parameter as string;

            if (string.IsNullOrEmpty(formatString))
                formatString = value as string;

            return ThemedImageConverterHelper.GetImage(formatString, true);
        }

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            throw new NotImplementedException();
        }
    }
}