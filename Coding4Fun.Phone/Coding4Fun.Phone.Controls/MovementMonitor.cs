using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Coding4Fun.Phone.Controls
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
			Monitor.SetValue(Grid.RowProperty, int.MaxValue - 1);
			Monitor.SetValue(Grid.ColumnProperty, int.MaxValue - 1);

            Monitor.ManipulationStarted += MonitorManipulationStarted;
            Monitor.ManipulationDelta += MonitorManipulationDelta;

            panel.Children.Add(Monitor);
        }

        void MonitorManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (Movement != null)
                Movement(this, new MovementMonitorEventArgs { 
                    X = _xOffsetStartValue + e.CumulativeManipulation.Translation.X,
                    Y = _yOffsetStartValue + e.CumulativeManipulation.Translation.Y
                });
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
        }
    }
}
