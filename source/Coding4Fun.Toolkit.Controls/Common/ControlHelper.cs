using System;

#if WINDOWS_STORE || WINDOWS_PHONE_APP
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
#if WINDOWS_STORE || WINDOWS_PHONE_APP
        static internal int MagicSpacingNumber = 10;
#elif WINDOWS_PHONE
		static internal int MagicSpacingNumber = 12;
#endif

		[Obsolete("Made into extension, Moved to Coding4Fun.Toolkit.dll now, Namespace is System, ")]
        public static double CheckBound(double value, double max)
        {
			return value.CheckBound(max);
        }

		[Obsolete("Made into extension, Moved to Coding4Fun.Toolkit.dll now, Namespace is System, ")]
		public static double CheckBound(double value, double min, double max)
        {
			return value.CheckBound(min, max);
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

#if WINDOWS_STORE || WINDOWS_PHONE_APP
            Storyboard.SetTargetProperty(doubleAni, propertyPath);
#elif WINDOWS_PHONE
			Storyboard.SetTargetProperty(doubleAni, new PropertyPath(propertyPath));
#endif

			sb.Children.Add(doubleAni);
		}
    }
}
