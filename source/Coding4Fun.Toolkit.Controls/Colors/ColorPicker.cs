using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Coding4Fun.Toolkit.Controls.Binding;
using Coding4Fun.Toolkit.Controls.Common;

// This is a heavily modified version based on their ColorPicker sample by 
// Author: Page Brooks
// Website: http://www.pagebrooks.com
namespace Coding4Fun.Toolkit.Controls
{
    public class ColorPicker : ColorBaseControl
    {
        bool _fromMovement;
		bool _adjustingColor;

        Point _position;

        #region controls on template
        protected Grid SampleSelector;
        private const string SampleSelectorName = "SampleSelector";

        protected Rectangle SelectedHueColor;
        private const string SelectedHueColorName = "SelectedHueColor";

        protected ColorSlider ColorSlider;
        private const string ColorSliderName = "ColorSlider";

        private MovementMonitor _monitor;
        private const string BodyName = "Body";
        #endregion

        public ColorPicker()
        {
            DefaultStyleKey = typeof (ColorPicker);

			PreventScrollBinding.SetIsEnabled(this, true);

            SizeChanged += ColorPicker_SizeChanged;
			IsEnabledChanged += ColorSlider_IsEnabledChanged;
			Loaded += ColorPicker_Loaded;
        }

		void ColorPicker_Loaded(object sender, RoutedEventArgs e)
		{
			IsEnabledVisualStateUpdate();
		}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SampleSelector = GetTemplateChild(SampleSelectorName) as Grid;
            SelectedHueColor = GetTemplateChild(SelectedHueColorName) as Rectangle;

            var body = GetTemplateChild(BodyName) as Grid;

            if (body != null)
            {
                _monitor = new MovementMonitor();
                _monitor.Movement += _monitor_Movement;
                _monitor.MonitorControl(body);
            }

            ColorSlider = GetTemplateChild(ColorSliderName) as ColorSlider;

            if (ColorSlider != null)
            {
                if (Thumb == null)
                    Thumb = new ColorSliderThumb();

                ColorSlider.ColorChanged += ColorSlider_ColorChanged;

                if(SelectedHueColor != null)
                {
                    var binding = new System.Windows.Data.Binding
                                      {
                                          Source = ColorSlider,
                                          Path = new PropertyPath("SolidColorBrush"),
                                      };

                    SelectedHueColor.SetBinding(Shape.FillProperty, binding);
                }
            }
        }

        #region events
		void ColorSlider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			IsEnabledVisualStateUpdate();
		}
        void ColorSlider_ColorChanged(object sender, Color color)
		{
            UpdateSample();
        }

        void ColorPicker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // setting UX up
            // setting color will call UpdateLayoutBasedOnColor
            // if we know the color, we must update UpdateLayoutBasedOnColor manually
            if (Color.A == 0 && Color.R == 0 && Color.G == 0 && Color.B == 0)
                Color = ColorSlider.Color;
            else
                UpdateLayoutBasedOnColor();
        }
        #endregion

        void _monitor_Movement(object sender, MovementMonitorEventArgs e)
        {
            _position.X = e.X;
            _position.Y = e.Y;
            
            UpdateSample();
        }

        private void UpdateSample()
        {
			_fromMovement = true;
            SetSampleLocation();

            var saturation = (float)(_position.X / SelectedHueColor.ActualWidth);
            var value = (float)(1 - (_position.Y / SelectedHueColor.ActualHeight));

			if(!_adjustingColor)
	            ColorChanging(ColorSpace.ConvertHsvToRgb(ColorSlider.Color.GetHue(), saturation, value));
			
			_fromMovement = false;
        }

        private void SetSampleLocation()
        {
            var sampleSelectorSize = SampleSelector.ActualHeight;

            var height = SelectedHueColor.ActualHeight;
            var width = SelectedHueColor.ActualWidth;

            _position.X = Numbers.CheckBound(_position.X, width);
			_position.Y = Numbers.CheckBound(_position.Y, height);

            var sampleLeft = _position.X - sampleSelectorSize;
            var sampleTop = _position.Y - sampleSelectorSize;

			sampleLeft = Numbers.CheckBound(sampleLeft, width);
			sampleTop = Numbers.CheckBound(sampleTop, height);

            SampleSelector.Margin = new Thickness(sampleLeft, sampleTop, 0, 0);
        }

        protected internal override void UpdateLayoutBasedOnColor()
        {
            if (_fromMovement || SelectedHueColor == null)
                return;

            base.UpdateLayoutBasedOnColor();

            var hsv = Color.GetHSV();

			if (ColorSlider != null)
			{
				_adjustingColor = true;

				ColorSlider.Color = ColorSpace.GetColorFromHueValue((int) hsv.Hue);
				
				_adjustingColor = false;
			}

        	_position.X = hsv.Saturation * SelectedHueColor.ActualWidth;
            _position.Y = (1 - hsv.Value) * SelectedHueColor.ActualHeight;
            SetSampleLocation();
        }

		private void IsEnabledVisualStateUpdate()
		{
			VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
		}

		#region dependency properties
		public object Thumb
        {
            get { return (object)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbProperty =
            DependencyProperty.Register("Thumb", typeof(object), typeof(ColorPicker), new PropertyMetadata(null));
		#endregion
	}
}
