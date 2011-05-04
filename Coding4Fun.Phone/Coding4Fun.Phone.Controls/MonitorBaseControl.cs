using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Coding4Fun.Phone.Controls
{
    public abstract class MonitorBaseControl : Control
    {
        protected Rectangle Monitor;
        private const string MonitorName = "Monitor";

        double _xOffsetValue;
        double _yOffsetValue;
        double _xOffsetStartValue;
        double _yOffsetStartValue;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Monitor = GetTemplateChild(MonitorName) as Rectangle;

            if (Monitor != null)
            {
                Monitor.ManipulationStarted += MonitorManipulationStarted;
                Monitor.ManipulationDelta += MonitorManipulationDelta;
            }
        }

        void MonitorManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            _xOffsetValue = _xOffsetStartValue + e.CumulativeManipulation.Translation.X;
            _yOffsetValue = _yOffsetStartValue + e.CumulativeManipulation.Translation.Y;

            UpdateSampleBasedOnManipulation(_xOffsetValue, _yOffsetValue);
        }

        void MonitorManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _xOffsetValue = _xOffsetStartValue = e.ManipulationOrigin.X;
            _yOffsetValue = _yOffsetStartValue = e.ManipulationOrigin.Y;

            UpdateSampleBasedOnManipulation(_xOffsetStartValue, _yOffsetStartValue);
        }

        protected void UpdateSampleBasedOnManipulation()
        {
            UpdateSampleBasedOnManipulation(_xOffsetValue, _yOffsetValue);
        }

        protected internal virtual void UpdateSampleBasedOnManipulation(double x, double y)
        {
        }

        protected internal double CheckBound(double value, double min, double max)
        {
            if (value <= min)
                value = min;
            else if (value >= max)
                value = max;

            return value;
        }
    }
}
