using System;

namespace Coding4Fun.Toolkit.Controls.Common
{
	[Obsolete("Moved to Coding4Fun.Toolkit.dll now, Namespace is System, ")]
    public static class TimeSpanExtensions
    {
		public static TimeSpan CheckBound(this TimeSpan value, TimeSpan maximum)
		{
			return System.TimeSpanExtensions.CheckBound(value, maximum);
        }

		public static TimeSpan CheckBound(this TimeSpan value, TimeSpan minimum, TimeSpan maximum)
        {
			return System.TimeSpanExtensions.CheckBound(value, minimum, maximum);
        }
    }
}