using System;
using System.Globalization;
using System.Windows;

namespace testSliderWinPhone8
{
	public class DoubleToGridLengthConverter : ValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			if (value == null)
				return new GridLength();

			double returnValue = 0;

			double.TryParse(value.ToString(), out returnValue);

			return new GridLength(returnValue);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			return null;
		}
	}
}
