using System.Windows.Input;
using System.Windows.Shapes;

namespace Coding4Fun.Phone.Controls
{
    public abstract class ColorMonitorBaseControl : ColorBaseControl
    {
        protected Rectangle ColorMonitor;
        private const string ColorMonitorName = "ColorMonitor";

        double _xOffsetValue;
        double _yOffsetValue;
        double _xOffsetStartValue;
        double _yOffsetStartValue;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ColorMonitor = GetTemplateChild(ColorMonitorName) as Rectangle;

            if (ColorMonitor != null)
            {
                ColorMonitor.ManipulationStarted += ColorMonitor_ManipulationStarted;
                ColorMonitor.ManipulationDelta += ColorMonitor_ManipulationDelta;
            }
        }

        void ColorMonitor_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            _xOffsetValue = _xOffsetStartValue + e.CumulativeManipulation.Translation.X;
            _yOffsetValue = _yOffsetStartValue + e.CumulativeManipulation.Translation.Y;

            UpdateSample(_xOffsetValue, _yOffsetValue);
        }

        void ColorMonitor_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _xOffsetValue = _xOffsetStartValue = e.ManipulationOrigin.X;
            _yOffsetValue = _yOffsetStartValue = e.ManipulationOrigin.Y;

            UpdateSample(_xOffsetStartValue, _yOffsetStartValue);
        }

        protected void UpdateSample()
        {
            UpdateSample(_xOffsetValue, _yOffsetValue);
        }

        protected internal virtual void UpdateSample(double x, double y)
        {
        }
        
        protected internal double CheckMarginBound(double value, double max)
        {
            if (value < 0)
                value = 0;
            else if (value > max)
                value = max;

            return value;
        }
    }
}
