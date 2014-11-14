#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#elif WINDOWS_PHONE
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#endif

namespace Coding4Fun.Toolkit.Controls.Common
{
	public static class ApplicationSpace
	{
#if WINDOWS_STORE || WINDOWS_PHONE_APP
		const int DefaultLogicalppi = 96;
		const int Percent = 100; 
#endif

        /// <summary>
        /// Gets the application scale factor 
        /// </summary>
        /// <returns>the scale factor (100 = 100%)</returns>
		public static int ScaleFactor()
		{
#if WIN81
            var displayInformation = DisplayInformation.GetForCurrentView();

            return (int)displayInformation.ResolutionScale;
#elif WINDOWS_STORE
	// http://code.msdn.microsoft.com/windowsapps/Scaling-sample-cf072f4f/sourcecode?fileId=43958&pathId=589460989
				return (int) ((DisplayProperties.LogicalDpi * Percent) / DefaultLogicalppi);
#elif WINDOWS_PHONE_APP
            var displayInformation = DisplayInformation.GetForCurrentView();

            return (int)(displayInformation.RawPixelsPerViewPixel * Percent);
#elif WINDOWS_PHONE
#if WP8
			return Application.Current.Host.Content.ScaleFactor;
#else
            return 100;
#endif
#endif
		}

		public static Frame RootFrame
		{
			get
			{
#if WINDOWS_STORE || WINDOWS_PHONE_APP
				var rootFrame = Window.Current.Content as Frame;
#elif WINDOWS_PHONE
				var rootFrame = Application.Current.RootVisual as Frame;
#endif

				return rootFrame;
			}

		}

		public static bool IsDesignMode
		{
			get
			{
#if WINDOWS_STORE || WINDOWS_PHONE_APP
				return DesignMode.DesignModeEnabled;
#elif WINDOWS_PHONE
				return DesignerProperties.IsInDesignTool;
#endif
			}
		}

		public static
#if WINDOWS_STORE || WINDOWS_PHONE_APP
            CoreDispatcher
#elif WINDOWS_PHONE
			Dispatcher
#endif
		CurrentDispatcher
		{
			get
            {
#if WINDOWS_STORE || WINDOWS_PHONE_APP
                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
#elif WINDOWS_PHONE
				var dispatcher = Deployment.Current.Dispatcher;
#endif

				return dispatcher;
			}
		}
	}
}