using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

using Coding4Fun.Toolkit.Controls.Common;
using testSliderWinPhone8;

// This is a heavily modified version based on ColorSlider in their ColorPicker sample by 
// Author: Page Brooks
// Website: http://www.pagebrooks.com
namespace Coding4Fun.Toolkit.Controls
{
    public class ColorSlider2 : ColorBaseControl
    {
        const double HueSelectorSize = 24;
        bool _fromSliderChange;

        #region controls on template
        protected Grid Body;
        private const string BodyName = "Body";

		private Rectangle _selectedColor;
		private const string SelectedColorName = "HorizontalSelectedColor";

		private SuperSlider _slider;
//		private SuperSliderUpdate _slider;
        private const string SliderName = "HorizontalSlider";
        #endregion

        public ColorSlider2()
        {
            DefaultStyleKey = typeof(ColorSlider2);

            IsEnabledChanged += SuperSlider_IsEnabledChanged;
	    }

	    public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Body = GetTemplateChild(BodyName) as Grid;

			_slider = GetTemplateChild(SliderName) as SuperSlider; 
			//_slider = GetTemplateChild(SliderName) as SuperSliderUpdate;

            _selectedColor = GetTemplateChild(SelectedColorName) as Rectangle;

            if (_slider != null)
            {
	            _slider.ApplyTemplate();
				_slider.ValueChanged += Slider_ValueChanged;

                if (Color.A == 0 && Color.R == 0 && Color.G == 0 && Color.B == 0)
                    Color = System.Windows.Media.Color.FromArgb(255, 6, 255, 0);
                else
                    UpdateLayoutBasedOnColor();
            }

			if (Thumb == null)
				Thumb = new ColorSliderThumb();

			SizeChanged += UserControl_SizeChanged;

			IsEnabledVisualStateUpdate();
        }

        void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetColorFromSlider(e.NewValue);
        }

        private void SetColorFromSlider(double value)
        {
            _fromSliderChange = true;
            ColorChanging(ColorSpace.GetColorFromHueValue((int)value % 360));
            _fromSliderChange = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
			if (double.IsNaN(DefaultSize))
			{
				var isVert = Orientation == Orientation.Vertical;

				DefaultSize = isVert ? ActualWidth : ActualHeight;
			}

			if (double.IsNaN(SelectedColorSize))
			{
				SelectedColorSize = DefaultSize;
			}

            AdjustLayoutBasedOnOrientation();
        }

        void SuperSlider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsEnabledVisualStateUpdate();
        }

        #region dependency properties


        public object Thumb
        {
            get { return (object)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbProperty =
			DependencyProperty.Register("Thumb", typeof(object), typeof(ColorSlider2), new PropertyMetadata(null));



		public double DefaultSize
		{
			get { return (double)GetValue(DefaultSizeProperty); }
			set { SetValue(DefaultSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DefaultSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DefaultSizeProperty =
			DependencyProperty.Register("DefaultSize", typeof(double), typeof(ColorSlider2), new PropertyMetadata(double.NaN));



		public double SelectedColorSize
		{
			get { return (double)GetValue(SelectedColorSizeProperty); }
			set { SetValue(SelectedColorSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedColorSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedColorSizeProperty =
			DependencyProperty.Register("SelectedColorSize", typeof(double), typeof(ColorSlider2), new PropertyMetadata(double.NaN));



        public bool IsColorVisible
        {
            get { return (bool)GetValue(IsColorVisibleProperty); }
            set { SetValue(IsColorVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsColorVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsColorVisibleProperty =
			DependencyProperty.Register("IsColorVisible", typeof(bool), typeof(ColorSlider2), new PropertyMetadata(true, OnIsColorVisibleChanged));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
			DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ColorSlider2), new PropertyMetadata(Orientation.Vertical, OnOrientationPropertyChanged));
        #endregion

        private static void OnIsColorVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
			var slider = d as ColorSlider2;

            if (slider != null)
                slider.AdjustLayoutBasedOnOrientation();
        }

        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
			var slider = d as ColorSlider2;
            
            if (slider != null)
                slider.AdjustLayoutBasedOnOrientation();
        }

        private void AdjustLayoutBasedOnOrientation()
        {
			

//			if (Body == null ||
//				_slider == null ||
//				_selectedColor == null)
//				return;

			var isVert = Orientation == Orientation.Vertical;

//			IsEnabledVisualStateUpdate();
            
//			Body.RowDefinitions.Clear();
//			Body.ColumnDefinitions.Clear();

//			if (isVert)
//			{
//				Body.RowDefinitions.Add(new RowDefinition());
//				Body.RowDefinitions.Add(new RowDefinition());
//			}
//			else
//			{
//				Body.ColumnDefinitions.Add(new ColumnDefinition());
//				Body.ColumnDefinitions.Add(new ColumnDefinition());
//			}

////            var thumb = ((FrameworkElement)_slider.Thumb);
			var thumb = Thumb as FrameworkElement;

			if (thumb != null)
			{
				thumb.Height = isVert ? HueSelectorSize : double.NaN;
				thumb.Width = isVert ? double.NaN : HueSelectorSize;
			}

//			_selectedColor.SetValue(Grid.RowProperty, isVert ? 1 : 0);
//			_selectedColor.SetValue(Grid.ColumnProperty, isVert ? 0 : 1);

//			var sliderWidth = _slider.ActualWidth;
//			var sliderHeight = _slider.ActualHeight;

//			var squareSize = isVert ? sliderWidth : sliderHeight;

//			//_selectedColor.Height = _selectedColor.Width = squareSize;

//			if (isVert)
//			{
//				Body.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
//				Body.RowDefinitions[1].Height = new GridLength(squareSize); // new GridLength(1, GridUnitType.Auto);
//			}
//			else
//			{
//				Body.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
//				Body.ColumnDefinitions[1].Width = new GridLength(squareSize); // new GridLength(1, GridUnitType.Auto);
//			}

			if(_selectedColor != null)
				_selectedColor.Visibility = (IsColorVisible) ? Visibility.Visible : Visibility.Collapsed;
        }

        protected internal override void UpdateLayoutBasedOnColor()
        {
            if (_fromSliderChange)
                return;

            base.UpdateLayoutBasedOnColor();

            if(_slider != null)
                _slider.Value = Color.GetHue();
        }

        private void IsEnabledVisualStateUpdate()
        {
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
			_slider.Background = IsEnabled ? ColorSpace.GetColorGradientBrush(Orientation) : ColorSpace.GetBlackAndWhiteGradientBrush(Orientation);
        }
    }
}
