using System;
using System.Globalization;
using System.Windows;

namespace testSliderWinPhone8
{
	public class ThicknessToGridLengthConverter : ValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			if (value == null || parameter == null)
				return new GridLength();

			var thicknessValue = (Thickness)value;

			var returnValue = 0d;
			switch (parameter.ToString().ToLowerInvariant())
			{
				default:
				case "top":
					returnValue = thicknessValue.Top;
					break;
				case "bottom":
					returnValue = thicknessValue.Bottom;
					break;
				case "right":
					returnValue = thicknessValue.Right;
					break;
				case "left":
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
