using System;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
#elif WINDOWS_PHONE
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
#endif

namespace Coding4Fun.Toolkit.Controls
{
    public class MovementMonitor
    {
        public event EventHandler<MovementMonitorEventArgs> Movement;

        protected Rectangle Monitor;

        double _xOffsetStartValue;
        double _yOffsetStartValue;

        public void MonitorControl(Panel panel)
        {
            Monitor = new Rectangle {Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))};
            Monitor.ManipulationMode = ManipulationModes.System;
			Monitor.SetValue(Grid.RowSpanProperty, int.MaxValue - 1);
			Monitor.SetValue(Grid.ColumnSpanProperty, int.MaxValue - 1);

#if WINDOWS_STORE || WINDOWS_PHONE_APP
            Monitor.PointerPressed += Monitor_PointerPressed;
            Monitor.PointerReleased += Monitor_PointerReleased;
            Monitor.PointerMoved += Monitor_PointerMoved;
#elif WINDOWS_PHONE
            Monitor.ManipulationStarted += MonitorManipulationStarted;
            Monitor.ManipulationDelta += MonitorManipulationDelta;
#endif
            panel.Children.Add(Monitor);
        }

        
#if WINDOWS_STORE || WINDOWS_PHONE_APP
        void Monitor_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(Monitor).Position;

            if (Movement != null)
                Movement(this, new MovementMonitorEventArgs { 
                    X = position.X,
                    Y = position.Y
                });

			e.Handled = true;
        }

        void Monitor_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(Monitor).Position;
            _xOffsetStartValue = position.X;
            _yOffsetStartValue = position.Y; 

            if (Movement != null)
                Movement(this, new MovementMonitorEventArgs
                {
                    X = _xOffsetStartValue,
                    Y = _yOffsetStartValue
                });

			e.Handled = true;
        }

        void Monitor_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!e.Pointer.IsInContact)
                return;

            var position = e.GetCurrentPoint(Monitor).Position;

            if (Movement != null)
                Movement(this, new MovementMonitorEventArgs
                {
                    X = position.X,
                    Y = position.Y
                });

            e.Handled = true;
        }
#elif WINDOWS_PHONE
        void MonitorManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (Movement != null)
                Movement(this, new MovementMonitorEventArgs { 
                    X = _xOffsetStartValue + e.CumulativeManipulation.Translation.X,
                    Y = _yOffsetStartValue + e.CumulativeManipulation.Translation.Y
                });

			e.Handled = true;
        }

        void MonitorManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _xOffsetStartValue = e.ManipulationOrigin.X;
            _yOffsetStartValue = e.ManipulationOrigin.Y; 

            if (Movement != null)
                Movement(this, new MovementMonitorEventArgs
                {
                    X = _xOffsetStartValue,
                    Y = _yOffsetStartValue
                });

			e.Handled = true;
        }
#endif
    }
}
