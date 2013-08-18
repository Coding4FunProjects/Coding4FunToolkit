namespace System
{
	public static class IntExtensions
    {
		public static double CheckBound(this int value, int maximum)
		{
			return CheckBound(value, 0, maximum);
		}

		public static double CheckBound(this int value, int minimum, int maximum)
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
    }
}