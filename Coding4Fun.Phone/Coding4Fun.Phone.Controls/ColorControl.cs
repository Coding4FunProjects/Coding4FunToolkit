using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Coding4Fun.Phone.Controls
{
    public abstract class ColorControl : Control
    {
        public delegate void ColorChangedHandler(object sender, Color color);
        public event ColorChangedHandler ColorChanged;

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

        #region dependency properties
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorControl), new PropertyMetadata(null));

        public SolidColorBrush SolidColorBrush
        {
            get { return (SolidColorBrush)GetValue(SolidColorBrushProperty); }
            private set { SetValue(SolidColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SolidColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SolidColorBrushProperty =
            DependencyProperty.Register("SolidColorBrush", typeof(SolidColorBrush), typeof(ColorControl), new PropertyMetadata(null));
        #endregion

        protected void UpdateSample()
        {
            UpdateSample(_xOffsetValue, _yOffsetValue);
        }

        protected internal virtual void UpdateSample(double x, double y)
        {
        }

        protected internal void ColorChanging(Color color)
        {
            Color = color;
            SolidColorBrush = new SolidColorBrush(Color);

            if (ColorChanged != null)
                ColorChanged(this, Color);
        }
    }
}
