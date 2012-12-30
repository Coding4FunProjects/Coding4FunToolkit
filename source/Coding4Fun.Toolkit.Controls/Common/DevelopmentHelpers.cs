#if WINDOWS_STORE

using Windows.ApplicationModel;

#elif WINDOWS_PHONE

using System.ComponentModel;

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

    }
}