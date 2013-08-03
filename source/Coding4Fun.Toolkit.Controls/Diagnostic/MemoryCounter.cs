using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

using Coding4Fun.Toolkit.Controls.Common;

using Microsoft.Phone.Info;

namespace Coding4Fun.Toolkit.Controls
{
    public class MemoryCounter : Control, IDisposable
    {
        private const float ByteToMega = 1024 * 1024;
        private DispatcherTimer _timer;

        private bool _threwException;

        public MemoryCounter()
        {
			if (Debugger.IsAttached)
			{
				DefaultStyleKey = typeof(MemoryCounter);
				DataContext = this;

				Loaded += ControlLoaded;
				Unloaded += ControlUnloaded;				
			}
			else
			{
				StopAndHide();
			}
        }

		public int UpdateInterval
        {
            get { return (int)GetValue(UpdateIntervalProperty); }
            set { SetValue(UpdateIntervalProperty, value); }
        }

        public static readonly DependencyProperty UpdateIntervalProperty =
            DependencyProperty.Register("UpdateInterval", typeof(int), typeof(MemoryCounter), new PropertyMetadata(1000, OnUpdateIntervalChanged));

        private static void OnUpdateIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
	        if (!Debugger.IsAttached) 
				return;

	        var sender = ((MemoryCounter) o);

	        if (sender != null && sender._timer != null)
		        sender._timer.Interval = TimeSpan.FromMilliseconds((int) e.NewValue);
        }

        public string CurrentMemory
        {
            get { return (string)GetValue(CurrentMemoryProperty); }
            set { SetValue(CurrentMemoryProperty, value); }
        }

        public static readonly DependencyProperty CurrentMemoryProperty =
            DependencyProperty.Register("CurrentMemory", typeof(string), typeof(MemoryCounter), new PropertyMetadata("0"));

        public string PeakMemory
        {
            get { return (string)GetValue(PeakMemoryProperty); }
            set { SetValue(PeakMemoryProperty, value); }
        }

        public static readonly DependencyProperty PeakMemoryProperty =
            DependencyProperty.Register("PeakMemory", typeof(string), typeof(MemoryCounter), new PropertyMetadata("0"));

        void TimerTick(object sender, EventArgs e)
        {
			if (!Debugger.IsAttached || _threwException)
			{
				StopAndHide();
			}

	        try
	        {
		        CurrentMemory = ((DeviceStatus.ApplicationCurrentMemoryUsage) / ByteToMega).ToString("#.00");
		        PeakMemory = ((DeviceStatus.ApplicationPeakMemoryUsage) / ByteToMega).ToString("#.00");

		        Debug.WriteLine("CALLING MEM: " + DateTime.Now);
	        }
	        catch (Exception)
	        {
		        _threwException = true;
				_timer.Stop();
	        }
        }

	    private void StopAndHide()
	    {
			if(_timer != null)
				_timer.Stop();

		    Visibility = Visibility.Collapsed;
	    }


		void ControlLoaded(object sender, RoutedEventArgs e)
		{
			if (ApplicationSpace.IsDesignMode)
				return;

			_timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(UpdateInterval) };
			_timer.Tick += TimerTick;
			_timer.Start();

			var rootFrame = ApplicationSpace.RootFrame;

			if (rootFrame == null)
				return;

			rootFrame.Navigated -= FrameNavigated;
			rootFrame.Navigated += FrameNavigated;
		}

		#region control unloaded
		void ControlUnloaded(object sender, RoutedEventArgs e)
		{
			Dispose();
		}

		void FrameNavigated(object sender, NavigationEventArgs e)
		{
#if WINDOWS_PHONE
			if (e.IsNavigationInitiator)
#endif
			{
				Dispose();
			}
		}

		public void Dispose()
		{
			var rootFrame = ApplicationSpace.RootFrame;

			if (rootFrame != null)
				rootFrame.Navigated -= FrameNavigated;

			if (_timer != null)
			{
				_timer.Stop();
				_timer.Tick -= TimerTick;

				_timer = null;
			}
		}
		#endregion

    }
}
