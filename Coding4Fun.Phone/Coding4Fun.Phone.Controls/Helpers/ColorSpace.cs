using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// This is a modified version based on their ColorPicker sample by 
// Author: Page Brooks
// Website: http://www.pagebrooks.com
// RSS Feed: http://feeds.pagebrooks.com/pagebrooks
namespace Coding4Fun.Phone.Controls.Helpers
{
    public class ColorSpace
    {
        private const byte Min = 0;
        private const byte Max = 255;
        private const byte Alpha = 255;
        const int GradientStops = 6;
        const double NegatedGradientStops = 1 / (float)GradientStops;

        public static LinearGradientBrush GetGradientBrush(Orientation orientation)
        {
            var brush = new LinearGradientBrush();

            //<GradientStop Offset="0.00" Color="#ffff0000"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 0, Color = Color.FromArgb(255, 255, 0, 0) });
            //<GradientStop Offset="0.166666" Color="#ffffff00"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 1, Color = Color.FromArgb(255, 255, 255, 0) });
            //<GradientStop Offset="0.333333" Color="#ff00ff00"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 2, Color = Color.FromArgb(255, 0, 255, 0) });
            //<GradientStop Offset="0.50" Color="#ff00ffff"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 3, Color = Color.FromArgb(255, 0, 255, 255) });
            //<GradientStop Offset="0.666666" Color="#ff0000ff"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 4, Color = Color.FromArgb(255, 0, 0, 255) });
            //<GradientStop Offset="0.833333" Color="#ffff00ff"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 5, Color = Color.FromArgb(255, 255, 0, 255) });
            //<GradientStop Offset="1.00" Color="#ffff0000"/>
            brush.GradientStops.Add(new GradientStop { Offset = NegatedGradientStops * 6, Color = Color.FromArgb(255, 255, 0, 0) });

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

        public static Color GetHueColorFromPosition(int position)
        {
            position *= GradientStops;
            var mod = (byte)(position % Max);
            var diff = (byte)(Max - mod);
            
            switch (position / Max)
            {
                case 0: return Color.FromArgb(Alpha, Max, mod, Min);
                case 1: return Color.FromArgb(Alpha, diff, Max, Min);
                case 2: return Color.FromArgb(Alpha, Min, Max, mod);
                case 3: return Color.FromArgb(Alpha, Min, diff, Max);
                case 4: return Color.FromArgb(Alpha, mod, Min, Max);
                case 5: return Color.FromArgb(Alpha, Max, Min, diff);
                default: return Colors.Black;
            }
        }

        public static int GetPositionFromHueColor(Color color)
        {
            var mod = int.MinValue;
            var diff = int.MinValue;
            var switchCase = int.MinValue;
            // reversing logic from GetHueColorFromPosition
            if (color.R == Max)
            {
                // covers case 0 and case 5
                if (color.B == Min)
                {
                    mod = color.G;
                    switchCase = 0;
                }
                else
                {
                    diff = color.B;
                    switchCase = 5;
                }
            }
            else if (color.G == Max)
            {
                // covers case 1 and 2
                if (color.B == Min)
                {
                    diff = color.R;
                    switchCase = 1;
                }
                else
                {
                    mod = color.B;
                    switchCase = 2;
                }
            }
            else if (color.B == Max)
            {
                // covers case 3 and case 4
                if (color.R == Min)
                {
                    diff = color.G;
                    switchCase = 3;
                }
                else
                {
                    mod = color.R;
                    switchCase = 4;
                }
            }

            if (switchCase == int.MinValue)
                return 0;

            //switch(position / Max)
            var position = switchCase*255;

            //var mod = (byte)(position % Max);
            if (mod == int.MinValue)
            {
                //var diff = Max - mod;
                // -diff + Max = mod

                mod = -diff + Max;
            }

            //position *= GradientStops;
            return (position + mod)/GradientStops;
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
                var a = (byte)Math.Round(Max * value * (1.0 - saturation));
                var b = (byte)Math.Round(Max * value * (1.0 - (saturation * (hue - hueFloor))));
                var c = (byte)Math.Round(Max * value * (1.0 - (saturation * (1.0 - (hue - hueFloor)))));
                var d = (byte)Math.Round(Max * value);

                switch (hueFloor)
                {
                    case 0:
                        return Color.FromArgb(Max, d, c, a);
                    case 1:
                        return Color.FromArgb(Max, b, d, a);
                    case 2:
                        return Color.FromArgb(Max, a, d, c);
                    case 3:
                        return Color.FromArgb(Max, a, b, d);
                    case 4:
                        return Color.FromArgb(Max, c, a, d);
                    case 5:
                        return Color.FromArgb(Max, d, a, b);
                    default:
                        return Color.FromArgb(0, 0, 0, 0);
                }
            }

            var temp = (byte) (value*Max);
            return Color.FromArgb(255, temp, temp, temp);
        }

        public static float CalculateHue(Color color)
        {
            if (color.R == color.G && color.G == color.B)
                return 0;

            var r = color.R / 255f;
            var g = color.G / 255f;
            var b = color.B / 255f;

            float hue;

            var min = Math.Min(Math.Min(r, g), b);
            var max = Math.Max(Math.Max(r, g), b);

            var delta = max - min;

            if (r == max)
                hue = (g - b) / delta; // between yellow & magenta
            else if (g == max)
                hue = 2 + (b - r) / delta; // between cyan & yellow
            else
                hue = 4 + (r - g) / delta; // between magenta & cyan

            hue *= 60; // degrees

            if (hue < 0)
                hue += 360;

            return hue;
        }
    }
}

