using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// This is a modified version based on ColorPicker sample by 
// Author: Page Brooks
// Website: http://www.pagebrooks.com
// RSS Feed: http://feeds.pagebrooks.com/pagebrooks
namespace Coding4Fun.Phone.Controls.Helpers
{
    public class ColorSpace
    {
        private const byte MinValue = 0;
        private const byte MaxValue = 255;
        private const byte DefaultAlphaValue = 255;

		private static readonly Color[] ColorGradients = 
			new[] {Color.FromArgb(255, 255, 0, 0),
				Color.FromArgb(255, 255, 255, 0),
				Color.FromArgb(255, 0, 255, 0),
				Color.FromArgb(255, 0, 255, 255),
				Color.FromArgb(255, 0, 0, 255),
				Color.FromArgb(255, 255, 0, 255)};

		// would like to have this computed 
		// values are from using paint.net and 
		// doing a black and white filter
		private static readonly Color[] BlackAndWhiteGradients =
			new[] {Color.FromArgb(255, 76, 76, 76),
				Color.FromArgb(255, 225, 225, 225),
				Color.FromArgb(255, 149, 149, 149),
				Color.FromArgb(255, 178, 178, 178),
				Color.FromArgb(255, 29, 29, 29),
				Color.FromArgb(255, 105, 105, 105)};

        public static LinearGradientBrush GetColorGradientBrush(Orientation orientation)
        {
			return CreateGradientBrush(orientation, ColorGradients);
        }

		public static LinearGradientBrush GetBlackAndWhiteGradientBrush(Orientation orientation)
		{
			return CreateGradientBrush(orientation, BlackAndWhiteGradients);
		}

		private static LinearGradientBrush CreateGradientBrush(Orientation orientation, params Color[] colors)
		{
		    var brush = new LinearGradientBrush();
			var negatedStops = 1 / (float) colors.Length;

			for (var i = 0; i < colors.Length; i++)
			{
				brush.GradientStops.Add(new GradientStop { Offset = negatedStops * i, Color = colors[i] });
			}

			// creating the full loop
			brush.GradientStops.Add(new GradientStop { Offset = negatedStops * colors.Length, Color = colors[0] });

            if (orientation == Orientation.Vertical)
            {
                brush.StartPoint = new Point(0, 1);
                brush.EndPoint = new Point();
            }
            else
            {
                brush.EndPoint = new Point(1, 0);
            }

            return brush;
        }
		
    	public static Color GetColorFromHueValue(float position)
        {
            position /= 360f;

			position *= ColorGradients.Length * 255;  // I know there are 6 stops in the 
            var mod = (byte)(position % MaxValue);
            var diff = (byte)(MaxValue - mod);
            
            switch ((int)position / MaxValue)
            {
                case 0: return Color.FromArgb(DefaultAlphaValue, MaxValue, mod, MinValue);
                case 1: return Color.FromArgb(DefaultAlphaValue, diff, MaxValue, MinValue);
                case 2: return Color.FromArgb(DefaultAlphaValue, MinValue, MaxValue, mod);
                case 3: return Color.FromArgb(DefaultAlphaValue, MinValue, diff, MaxValue);
                case 4: return Color.FromArgb(DefaultAlphaValue, mod, MinValue, MaxValue);
                case 5: return Color.FromArgb(DefaultAlphaValue, MaxValue, MinValue, diff);
                default: return Colors.Black;
            }
        }

        public static string GetHexCode(Color c)
        {
            return string.Format("#{0}{1}{2}",
                c.R.ToString("X2"),
                c.G.ToString("X2"),
                c.B.ToString("X2"));
        }

        // Algorithm ported from: http://www.colorjack.com/software/dhtml+color+picker.html
        public static Color ConvertHsvToRgb(float hue, float saturation, float value)
        {
            hue = hue/360f;

            if (saturation > 0)
            {
                if (hue >= 1)
                    hue = 0;

                hue = 6 * hue;

                var hueFloor = (int)Math.Floor(hue);
                var a = (byte)Math.Round(MaxValue * value * (1.0 - saturation));
                var b = (byte)Math.Round(MaxValue * value * (1.0 - (saturation * (hue - hueFloor))));
                var c = (byte)Math.Round(MaxValue * value * (1.0 - (saturation * (1.0 - (hue - hueFloor)))));
                var d = (byte)Math.Round(MaxValue * value);

                switch (hueFloor)
                {
                    case 0:
                        return Color.FromArgb(MaxValue, d, c, a);
                    case 1:
                        return Color.FromArgb(MaxValue, b, d, a);
                    case 2:
                        return Color.FromArgb(MaxValue, a, d, c);
                    case 3:
                        return Color.FromArgb(MaxValue, a, b, d);
                    case 4:
                        return Color.FromArgb(MaxValue, c, a, d);
                    case 5:
                        return Color.FromArgb(MaxValue, d, a, b);
                    default:
                        return Color.FromArgb(0, 0, 0, 0);
                }
            }

            var temp = (byte) (value*MaxValue);
            return Color.FromArgb(255, temp, temp, temp);
        }
    }
}

