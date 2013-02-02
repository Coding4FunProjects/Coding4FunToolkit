namespace Coding4Fun.Phone.Controls.Helpers
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
    }
}
