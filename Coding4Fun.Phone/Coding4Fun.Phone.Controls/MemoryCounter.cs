using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using Microsoft.Phone.Info;

namespace Coding4Fun.Phone.Controls
{
    public class MemoryCounter : Control
    {
        private const float ByteToMega = 1024 * 1024;
        private readonly DispatcherTimer _timer;

        private bool _threwException;

        public MemoryCounter()
        {
            if (Debugger.IsAttached)
            {
                DefaultStyleKey = typeof(MemoryCounter);
                DataContext = this;

                _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(UpdateInterval) };
                _timer.Tick += timer_Tick;
                _timer.Start();
            }
            else
                Visibility = Visibility.Collapsed;

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
            if (Debugger.IsAttached)
            {
                var sender = ((MemoryCounter) o);

                if (sender != null && sender._timer != null)
                    sender._timer.Interval = TimeSpan.FromMilliseconds((int) e.NewValue);
            }
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

        void timer_Tick(object sender, EventArgs e)
        {
            if (Debugger.IsAttached || _threwException)
            {
            	try
            	{
            		CurrentMemory = ((DeviceStatus.ApplicationCurrentMemoryUsage) / ByteToMega).ToString("#.00");
            		PeakMemory = ((DeviceStatus.ApplicationPeakMemoryUsage) / ByteToMega).ToString("#.00");
            	}
            	catch (Exception)
            	{
            		_threwException = true;
            		_timer.Stop();
            	}
            }
        }
    }
}
