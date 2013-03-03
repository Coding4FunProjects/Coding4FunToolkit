using System;

#if WINDOWS_STORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Media.Animation;

#endif

namespace Coding4Fun.Toolkit.Controls.Common
{
    public class ControlHelper
    {
        public static double CheckBound(double value, double max)
        {
            return CheckBound(value, 0, max);
        }

        public static double CheckBound(double value, double min, double max)
        {
            if (value <= min)
                value = min;
            else if (value >= max)
                value = max;

            return value;
        }

		public static void CreateDoubleAnimations(Storyboard sb, DependencyObject target, string propertyPath, double fromValue = 0, double toValue = 0, int speed = 500)
		{
			var doubleAni = new DoubleAnimation
			{
				To = toValue,
				From = fromValue,
				Duration = new Duration(TimeSpan.FromMilliseconds(speed)),
			};

			Storyboard.SetTarget(doubleAni, target);

#if WINDOWS_STORE
			Storyboard.SetTargetProperty(doubleAni, propertyPath);
#elif WINDOWS_PHONE
			Storyboard.SetTargetProperty(doubleAni, new PropertyPath(propertyPath));
#endif

			sb.Children.Add(doubleAni);
		}
    }
}
