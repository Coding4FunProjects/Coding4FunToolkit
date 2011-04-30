using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using SilverlightColorPicker;

// This is a heavily modified version based on their ColorPicker sample by 
// Author: Page Brooks
// Website: http://www.pagebrooks.com
namespace Coding4Fun.Phone.Controls
{
    public class ColorPicker : ColorControl
    {
        double _sampleSelectorSize = 10;

        private float _hue;

        #region controls on template
        protected Grid SampleSelector;
        private const string SampleSelectorName = "SampleSelector";

        protected Rectangle SelectedHueColor;
        private const string SelectedHueColorName = "SelectedHueColor";

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
            ColorSlider = GetTemplateChild(ColorSliderName) as ColorSlider;

            if (ColorSlider != null) 
                ColorSlider.ColorChanged += ColorSlider_ColorChanged;
        }

        #region events
        private void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            _sampleSelectorSize = SampleSelector.ActualHeight;

            UpdateSample(ColorMonitor.ActualWidth, 0);
        }

        void ColorSlider_ColorChanged(object sender, Color color)
        {
            _hue = ColorSpace.CalculateHue(color);
            SelectedHueColor.Fill = new SolidColorBrush(color);

            UpdateSample();
        }
        #endregion

        protected internal override void UpdateSample(double x, double y)
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

            ColorChanging(ColorSpace.ConvertHsvToRgb(_hue, saturation, value));
        }
    }
}
