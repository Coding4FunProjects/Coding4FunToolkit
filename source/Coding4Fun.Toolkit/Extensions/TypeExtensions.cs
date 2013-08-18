#if WINDOWS_STORE
using System.Reflection;

#endif

namespace System
{
	public static class TypeExtensions
	{
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