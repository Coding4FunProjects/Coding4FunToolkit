using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

using Microsoft.Xna.Framework;

namespace Coding4Fun.Toolkit.Audio
{
	public static class XnaFrameworkDispatcherService
	{
		private static Timer _threadTimer;
		
		static void TimerAction(object state)
		{
			UpdateService();
		}

		public static void UpdateService()
		{
			try
			{
				FrameworkDispatcher.Update();
			}
			catch (Exception ex0)
			{
				Debug.WriteLine(ex0);
			}
		}

		public static void StartService()
		{
			Application.Current.RootVisual.Dispatcher.BeginInvoke(()=>
				_threadTimer = new Timer(TimerAction, null, TimeSpan.FromMilliseconds(33), TimeSpan.FromMilliseconds(33)));
		}

		public static void StopService()
		{
			if(_threadTimer != null)
				_threadTimer.Dispose();

			_threadTimer = null;
		}
	}
}
