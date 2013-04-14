using System;
using System.Globalization;
using System.Windows;

namespace testMargin
{
	public class ThicknessToGridLengthConverter : ValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			if (value == null || parameter == null)
				return new GridLength();

			var targetProperty = Enum.Parse(typeof(ThicknessProperties), parameter.ToString());
			var thicknessValue = (Thickness)value;

			if (targetProperty == null)
				return new GridLength();

			var returnValue = 0d;
			switch ((ThicknessProperties)targetProperty)
			{
				default:
				case ThicknessProperties.Top:
					returnValue = thicknessValue.Top;
					break;
				case ThicknessProperties.Bottom:
					returnValue = thicknessValue.Bottom;
					break;
				case ThicknessProperties.Right:
					returnValue = thicknessValue.Right;
					break;
				case ThicknessProperties.Left:
					returnValue = thicknessValue.Left;
					break;
			}

			return new GridLength(returnValue);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			return null;
		}
	}
}
