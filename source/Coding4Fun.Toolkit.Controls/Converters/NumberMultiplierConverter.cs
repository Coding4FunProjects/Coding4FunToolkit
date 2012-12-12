using System;
using System.Globalization;

namespace Coding4Fun.Toolkit.Controls.Converters
{
	public class NumberMultiplierConverter : ValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			double initValue;
			double multiplier;

			double.TryParse(value.ToString(), out initValue);
			double.TryParse(parameter.ToString(), out multiplier);

			return initValue * multiplier;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			throw new NotImplementedException();
		}
	}
}