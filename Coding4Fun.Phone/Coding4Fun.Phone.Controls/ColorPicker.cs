using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using SilverlightColorPicker;

// This is a heavily modified version based on their ColorPicker sample by 
// Author: Page Brooks
// Website: http://www.pagebrooks.com
namespace Coding4Fun.Phone.Controls
{
    public class ColorPicker : Control
    {
        double _xOffsetValue;
        double _yOffsetValue;
        double _xOffsetStartValue;
        double _yOffsetStartValue;

        double _sampleSelectorSize = 10;

        public delegate void ColorSelectedHandler(object sender, Color color);
        public event ColorSelectedHandler ColorSelected;
        private float _hue;
        #region controls on template
        protected Grid SampleSelector;
        private const string SampleSelectorName = "SampleSelector";

        protected Rectangle SelectedHueColor;
        private const string SelectedHueColorName = "SelectedHueColor";

        protected Rectangle ColorMonitor;
        private const string ColorMonitorName = "ColorMonitor";

        protected ColorSlider ColorSlider;
        private const string ColorSliderName = "ColorSlider";
        #endregion

        public ColorPicker()
        {
            DefaultStyleKey = typeof (ColorPicker);

            Loaded += ColorPicker_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SampleSelector = GetTemplateChild(SampleSelectorName) as Grid;

            SelectedHueColor = GetTemplateChild(SelectedHueColorName) as Rectangle;
            ColorMonitor = GetTemplateChild(ColorMonitorName) as Rectangle;

            ColorSlider = GetTemplateChild(ColorSliderName) as ColorSlider;


            if (ColorMonitor != null)
            {
                ColorMonitor.ManipulationStarted += ColorMonitor_ManipulationStarted;
                ColorMonitor.ManipulationDelta += ColorMonitor_ManipulationDelta;
            }

            if (ColorSlider != null) 
                ColorSlider.ColorChanged += ColorSlider_ColorChanged;
        }

        #region events
        private void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            _sampleSelectorSize = SampleSelector.ActualHeight;

            _xOffsetValue = (int)ColorMonitor.ActualWidth;
            _yOffsetValue = 0;

            UpdateSample(_xOffsetValue, _yOffsetValue);
        }

        void ColorSlider_ColorChanged(object sender, Color color)
        {
            _hue = ColorSpace.CalculateHue(color);
            SelectedHueColor.Fill = new SolidColorBrush(color);

            UpdateSample(_xOffsetValue, _yOffsetValue);
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
        #endregion
        #region dependency properties
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker), new PropertyMetadata(null));

        public SolidColorBrush SolidColorBrush
        {
            get { return (SolidColorBrush)GetValue(SolidColorBrushProperty); }
            private set { SetValue(SolidColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SolidColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SolidColorBrushProperty =
            DependencyProperty.Register("SolidColorBrush", typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(null));
        #endregion

        private void UpdateSample(double x, double y)
        {
            var height = SelectedHueColor.ActualHeight;
            var width = SelectedHueColor.ActualWidth;

            if (x < 0)
                x = 0;
            else if (x > width)
                x = width;

            if (y < 0)
                y = 0;
            else if (y > height)
                y = height;

            var sampleLeft = x - (_sampleSelectorSize / 2);
            var sampleTop = y - (_sampleSelectorSize / 2);

            if (sampleLeft < 0)
                sampleLeft = 0;

            if (sampleTop < 0)
                sampleTop = 0;

            SampleSelector.Margin = new Thickness(sampleLeft, sampleTop, 0, 0);

            var saturation = (float)(x / width);
            var value = (float)(1 - (y / height));

            Color = ColorSpace.ConvertHsvToRgb(_hue, saturation, value);
            SolidColorBrush = new SolidColorBrush(Color);
            
            if (ColorSelected != null)
                ColorSelected(this, Color);
        }
    }
}
