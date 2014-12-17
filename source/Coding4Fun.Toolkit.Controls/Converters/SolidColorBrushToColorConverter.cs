using System;
using System.Globalization;

#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE
using System.Windows.Media;

#endif

namespace Coding4Fun.Toolkit.Controls.Converters
{
	public class SolidColorBrushToColorConverter : ValueConverter
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Specifies alphato control transparency</param>
        /// <param name="culture"></param>
        /// <param name="language"></param>
        /// <returns></returns>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			var brush = value as SolidColorBrush;

            Color c = Colors.Transparent;

			if (brush != null)
				c = brush.Color;

            if(parameter != null)
            {
                byte alpha;
                if(Byte.TryParse((string)parameter, out alpha))
                {
                    c.A = alpha;
                }
            }

            return c;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			throw new NotImplementedException();
		}
	}
}