using System;

namespace Coding4Fun.Toolkit.Controls.Common
{
	public static class FloatExtensions
    {
		public static double CheckBound(this float value, float maximum)
		{
			return CheckBound(value, 0, maximum);
		}

		public static double CheckBound(this float value, float minimum, float maximum)
		{
			if (value <= minimum)
			{
				value = minimum;
			}
			else if (value >= maximum)
			{
				value = maximum;
			}

			return value;
		}

	    /// <summary>
	    /// Tests equality with a certain amount of precision.  Default to smallest possible double
	    /// </summary>
	    /// <param name="a">first value</param>
	    /// <param name="b">second value</param> 
	    /// <param name="precision">optional, smallest possible double value</param>
	    /// <returns></returns>
		public static bool AlmostEquals(this float a, float b, double precision = float.Epsilon)
	    {
		    return Math.Abs(a - b) <= precision;
	    }
    }
}