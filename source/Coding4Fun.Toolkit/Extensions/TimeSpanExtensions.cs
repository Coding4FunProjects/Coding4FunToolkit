namespace System
{
    public static class TimeSpanExtensions
    {
		public static TimeSpan CheckBound(this TimeSpan value, TimeSpan maximum)
        {
			return CheckBound(value, default(TimeSpan), maximum);
        }

		public static TimeSpan CheckBound(this TimeSpan value, TimeSpan minimum, TimeSpan maximum)
        {
			if (value < minimum)
            {
				value = minimum;
            }
			else if (value > maximum)
            {
				value = maximum;
            }

            return value;
        }
    }
}