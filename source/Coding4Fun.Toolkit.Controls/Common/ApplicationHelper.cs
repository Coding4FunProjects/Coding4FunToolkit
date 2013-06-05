#if WINDOWS_STORE
using Windows.Graphics.Display;

#elif WINDOWS_PHONE
using System.Windows;

#endif

namespace Coding4Fun.Toolkit.Controls.Common
{
	public static class ApplicationSpace
	{
#if WINDOWS_STORE
		const int DefaultLogicalppi = 96;
		const int Percent = 100; 
#endif

		public static int ScaleFactor()
		{
			return 
#if WINDOWS_STORE
				// http://code.msdn.microsoft.com/windowsapps/Scaling-sample-cf072f4f/sourcecode?fileId=43958&pathId=589460989
				(int) ((DisplayProperties.LogicalDpi * Percent) / DefaultLogicalppi);
#elif WINDOWS_PHONE
#if WP8
				Application.Current.Host.Content.ScaleFactor;
#else
            100;
#endif
#endif
		}
	}
}