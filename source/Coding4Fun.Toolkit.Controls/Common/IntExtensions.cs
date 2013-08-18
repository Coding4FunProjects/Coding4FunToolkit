using System;

namespace Coding4Fun.Toolkit.Controls.Common
{
	[Obsolete("Moved to Coding4Fun.Toolkit.dll now, Namespace is System, ")]
	public static class IntExtensions
    {
		public static double CheckBound(this int value, int maximum)
		{
			return System.IntExtensions.CheckBound(value, maximum);
		}

		public static double CheckBound(this int value, int minimum, int maximum)
		{
			return System.IntExtensions.CheckBound(value, minimum, maximum);
		}
    }
}