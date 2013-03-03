using System;

using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

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
			Monitor.SetValue(Grid.RowSpanProperty, int.MaxValue - 1);
			Monitor.SetValue(Grid.ColumnSpanProperty, int.MaxValue - 1);

	        Monitor.ManipulationMode = ManipulationModes.All;
			Monitor.ManipulationStarted += MonitorManipulationStarted;
			Monitor.ManipulationDelta += MonitorManipulationDelta;

            panel.Children.Add(Monitor);
        }

		
        void MonitorManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (Movement != null)
                Movement(this, new MovementMonitorEventArgs { 
                    X = _xOffsetStartValue + e.Cumulative.Translation.X,
                    Y = _yOffsetStartValue + e.Cumulative.Translation.Y
                });

			e.Handled = true;
        }

        void MonitorManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _xOffsetStartValue = e.Position.X;
            _yOffsetStartValue = e.Position.Y; 
            
            if (Movement != null)
                Movement(this, new MovementMonitorEventArgs
                {
                    X = _xOffsetStartValue,
                    Y = _yOffsetStartValue
                });

			e.Handled = true;
        }
    }
}
