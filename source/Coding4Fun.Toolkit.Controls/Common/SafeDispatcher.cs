using System;
using System.Threading;

#if WINDOWS_STORE
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

#elif WINDOWS_PHONE
using System.Windows;

#endif

namespace Coding4Fun.Toolkit.Controls.Common
{
	public class SafeDispatcher
	{
		public static void Run(Action func)
		{
#if WINDOWS_STORE
			var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
#elif WINDOWS_PHONE
			var dispatcher = Deployment.Current.Dispatcher;
#endif

			if (dispatcher == null)
				return;

#if WINDOWS_STORE
			if (!dispatcher.HasThreadAccess)
#elif WINDOWS_PHONE
			if (!dispatcher.CheckAccess())
#endif
			{

#if WINDOWS_STORE
				dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => func());
#elif WINDOWS_PHONE
				dispatcher.BeginInvoke(func);
#endif
			}
			else
			{
				func();
			}
		}
	

		public static T Run<T>(Func<T> func)
		{
			var returnData = default(T);

#if WINDOWS_STORE
			var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
#elif WINDOWS_PHONE
			var dispatcher = Deployment.Current.Dispatcher;
#endif

			if (dispatcher == null)
				return returnData;

			

#if WINDOWS_STORE
			if (!dispatcher.HasThreadAccess)
#elif WINDOWS_PHONE
			if (!dispatcher.CheckAccess())
#endif
			{
				var holdMutex = new AutoResetEvent(true);

#if WINDOWS_STORE
				dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
					returnData = func()
					);
#elif WINDOWS_PHONE
				dispatcher.BeginInvoke(() =>
				{
					returnData = func();

					holdMutex.Set();

				}
					);
#endif
				holdMutex.Reset();
				holdMutex.WaitOne();
			}
			else
			{
				returnData = func();
			}

			return returnData;
		}
	}
}