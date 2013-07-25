using System;

#if WINDOWS_STORE
using System.Reflection;

#endif

using Coding4Fun.Toolkit.Controls.Common;

namespace Coding4Fun.Toolkit.Controls
{
	/// <summary>
	/// A static class providing methods for working with the visual tree.  
	/// </summary>
	public static class DevelopmentHelpers
	{
		[Obsolete("Moved to Coding4Fun.Toolkit.Controls.Common.ApplicationSpace")]
		public static bool IsDesignMode
		{
			get
			{
				return ApplicationSpace.IsDesignMode;
			}
		}

		public static bool IsTypeOf(this object target, Type type)
		{
#if WINDOWS_STORE
			return target.GetType().GetTypeInfo().IsSubclassOf(type);
#elif WINDOWS_PHONE
			return type.IsInstanceOfType(target);
#endif
		}

		public static bool IsTypeOf(this object target, object referenceObject)
		{
			return target.IsTypeOf(referenceObject.GetType());
		}
	}
}

namespace Coding4Fun.Toolkit.Controls.Common
{
    public static class TimeSpanHelper
    {
        public static TimeSpan CheckBound(this TimeSpan value, TimeSpan max)
        {
            return CheckBound(value, default(TimeSpan), max);
        }

        public static TimeSpan CheckBound(this TimeSpan value, TimeSpan min, TimeSpan max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }
    }
}