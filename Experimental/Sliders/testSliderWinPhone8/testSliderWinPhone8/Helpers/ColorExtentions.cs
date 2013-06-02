using System;

#if WINDOWS_STORE
using Windows.UI;
#elif WINDOWS_PHONE
using System.Windows.Media;
#endif

namespace Coding4Fun.Toolkit.Controls.Common
{
    public static class ColorExtentions
    {
        private static float Max(float val1, float val2, float val3)
        {
            return Math.Max(Math.Max(val1, val2), val3);
        }

        private static float Min(float val1, float val2, float val3)
        {
            return Math.Min(Math.Min(val1, val2), val3);
        }

        public static float GetHue(this Color color)
        {
            if (color.R == color.G && color.G == color.B)
                return 0;

            var r = color.R / 255f;
            var g = color.G / 255f;
            var b = color.B / 255f;

            float hue;

            var min = Min(r, g, b);
            var max = Max(r, g, b);

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

        public static HSV GetHSV(this Color color)
        {
            var hsv = new HSV
            {
                Hue = color.GetHue(),
                Saturation = color.GetSaturation(),
                Value = color.GetValue()
            };

            return hsv;
        }

        public static float GetSaturation(this Color color)
        {
            var r = color.R / 255f;
            var g = color.G / 255f;
            var b = color.B / 255f;

            var min = Min(r, g, b);
            var max = Max(r, g, b); 
            
            if (max == min)
                return 0;

            return (max == 0f) ? 0f : 1f - (1f * min / max);
            //var saturation = (max + min) / 2f;
            //if (saturation <= 0.5)
            //    return ((max - min) / (max + min));

            //return ((max - min) / ((2f - max) - min));
        }

        public static float GetValue(this Color color)
        {
            var r = color.R / 255f;
            var g = color.G / 255f;
            var b = color.B / 255f;

            return Max(r, g, b);
        }
    }

    public struct HSV
    {
        public float Hue;
        public float Saturation;
        public float Value;
    }
}
