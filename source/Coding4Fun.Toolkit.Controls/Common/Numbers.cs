using System.Linq;

namespace Coding4Fun.Toolkit.Controls
{
	public static class Numbers
	{
		public static float Max(params int[] numbers)
		{
			return numbers.Max();
		}

		public static float Min(params int[] numbers)
		{
			return numbers.Min();
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