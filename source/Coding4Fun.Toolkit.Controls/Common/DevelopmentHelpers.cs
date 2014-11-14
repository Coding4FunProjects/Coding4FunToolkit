using System;

#if WINDOWS_STORE || WINDOWS_PHONE_APP
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

		[Obsolete("Moved to Coding4Fun.Toolkit.dll now, Namespace is System")]
		public static bool IsTypeOf(this object target, Type type)
		{
			return System.TypeExtensions.IsTypeOf(target, type);
		}

		[Obsolete("Moved to Coding4Fun.Toolkit.dll now, Namespace is System")]
		public static bool IsTypeOf(this object target, object referenceObject)
		{
			return System.TypeExtensions.IsTypeOf(target, referenceObject);
		}
	}
}