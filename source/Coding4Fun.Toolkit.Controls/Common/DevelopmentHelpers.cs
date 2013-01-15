using System;

#if WINDOWS_STORE
using System.Reflection;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

#elif WINDOWS_PHONE
using System.ComponentModel;
using System.Windows;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	/// <summary>
	/// A static class providing methods for working with the visual tree.  
	/// </summary>
	public static class DevelopmentHelpers
	{
		public static bool IsDesignMode
		{
			get
			{
				return
#if WINDOWS_STORE
					DesignMode.DesignModeEnabled;
#elif WINDOWS_PHONE
					DesignerProperties.IsInDesignTool;
#endif
			}
		}

		public static bool IsTypeOf(this UIElement element, Type type)
		{
#if WINDOWS_STORE
			return element.GetType().GetTypeInfo().IsSubclassOf(type);
#elif WINDOWS_PHONE
			return type.IsInstanceOfType(element);
#endif
		}
	}
}