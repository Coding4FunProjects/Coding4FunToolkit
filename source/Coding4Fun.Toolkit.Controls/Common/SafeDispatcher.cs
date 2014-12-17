using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coding4Fun.Toolkit.Controls.Common
{
	public class SafeDispatcher
	{
		public static async Task Run(Action func)
		{
			var dispatcher = ApplicationSpace.CurrentDispatcher;

			if (dispatcher == null)
				return;

#if WINDOWS_STORE || WINDOWS_PHONE_APP
            if (!dispatcher.HasThreadAccess)
#elif WINDOWS_PHONE
			if (!dispatcher.CheckAccess())
#endif
            {

#if WINDOWS_STORE || WINDOWS_PHONE_APP
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => func());
#elif WINDOWS_PHONE
				dispatcher.BeginInvoke(func);
#endif
			}
			else
			{
				func();
			}
		}
	

		public static async Task<T> Run<T>(Func<T> func)
		{
			var returnData = default(T);

			var dispatcher = ApplicationSpace.CurrentDispatcher;

			if (dispatcher == null)
				return returnData;

#if WINDOWS_STORE || WINDOWS_PHONE_APP
            if (!dispatcher.HasThreadAccess)
#elif WINDOWS_PHONE
			if (!dispatcher.CheckAccess())
#endif
			{
				var holdMutex = new AutoResetEvent(true);

#if WINDOWS_STORE || WINDOWS_PHONE_APP
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
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