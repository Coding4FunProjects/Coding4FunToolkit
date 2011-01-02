using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Phone.Info;

namespace Coding4Fun.Phone.Controls
{
    public class MemoryCounter : Control
    {
        private const long ByteToMega = 1024 * 1024;
        private readonly DispatcherTimer _timer;

        public MemoryCounter()
        {
            DefaultStyleKey = typeof(MemoryCounter);
            DataContext = this;
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(UpdateInterval) };
                _timer.Tick += timer_Tick;
                _timer.Start();
            }
            else
#endif
                Visibility = Visibility.Collapsed;
        }

        public int UpdateInterval
        {
            get { return (int)GetValue(UpdateIntervalProperty); }
            set { SetValue(UpdateIntervalProperty, value); }
        }

        public static readonly DependencyProperty UpdateIntervalProperty =
            DependencyProperty.Register("UpdateInterval", typeof(int), typeof(MemoryCounter), new PropertyMetadata(100, OnUpdateIntervalChanged));

        private static void OnUpdateIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
#if DEBUG
            var sender = ((MemoryCounter)o);

            if (sender != null && sender._timer != null)
                sender._timer.Interval = TimeSpan.FromMilliseconds((int)e.NewValue);
#endif
        }

        public string CurrentMemory
        {
            get { return (string)GetValue(CurrentMemoryProperty); }
            set { SetValue(CurrentMemoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentMemory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMemoryProperty =
            DependencyProperty.Register("CurrentMemory", typeof(string), typeof(MemoryCounter), new PropertyMetadata("0"));

        public string PeakMemory
        {
            get { return (string)GetValue(PeakMemoryProperty); }
            set { SetValue(PeakMemoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PeakMemory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PeakMemoryProperty =
            DependencyProperty.Register("PeakMemory", typeof(string), typeof(MemoryCounter), new PropertyMetadata("0"));
#if DEBUG
        void timer_Tick(object sender, EventArgs e)
        {
            CurrentMemory = ((long)DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage") / ByteToMega).ToString();
            PeakMemory = ((long)DeviceExtendedProperties.GetValue("ApplicationPeakMemoryUsage") / ByteToMega).ToString();
        }
#endif
    }
}
