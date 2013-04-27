using System;
using System.Linq;

namespace Coding4Fun.Toolkit.Controls
{
	public static class Numbers
	{
		/// <summary>
		/// Tests equality with a certain amount of precision.  Default to smallest possible double
		/// </summary>
		/// <param name="a">first value</param>
		/// <param name="b">second value</param> 
		/// <param name="precision">optional, smallest possible double value</param>
		/// <returns></returns>
		public static bool AlmostEquals(this double a, double b, double precision = Double.Epsilon)
		{
			return Math.Abs(a - b) <= precision;
		}

		/// <summary>
		/// Tests equality with a certain amount of precision.  Default to smallest possible float
		/// </summary>
		/// <param name="a">first value</param>
		/// <param name="b">second value</param>
		/// <param name="precision">optional, defaults to smallest possible float</param>
		/// <returns></returns>
		public static bool AlmostEquals(this float a, float b, float precision = float.Epsilon)
		{
			return Math.Abs(a - b) <= precision;
		}

		public static float Max(params float[] numbers)
		{
			return numbers.Max();
		}

		public static float Min(params float[] numbers)
		{
			return numbers.Min();
		}

		public static double Max(params double[] numbers)
		{
			return numbers.Max();
		}

		public static double Min(params double[] numbers)
		{
			return numbers.Min();
		}
	}
}