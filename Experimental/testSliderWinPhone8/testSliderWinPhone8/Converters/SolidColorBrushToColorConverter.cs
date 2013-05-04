using System;
using System.Globalization;

#if WINDOWS_STORE
using Windows.UI;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE
using System.Windows.Media;

#endif
using testSliderWinPhone8;


namespace Coding4Fun.Toolkit.Controls.Converters
{
	public class SolidColorBrushToColorConverter : ValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			var brush = value as SolidColorBrush;

			if (brush != null)
				return brush.Color;

			return Colors.Transparent;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			throw new NotImplementedException();
		}
	}
}