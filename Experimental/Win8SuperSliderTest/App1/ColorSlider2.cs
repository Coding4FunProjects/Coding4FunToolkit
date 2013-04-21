
using App1;
using Coding4Fun.Toolkit.Controls.Common;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace Coding4Fun.Toolkit.Controls
{
	public class ColorSlider2 : ColorBaseControl
	{
		const double HueSelectorSize = 24;
		bool _fromSliderChange;

		#region controls on template
		protected Grid Body;
		private const string BodyName = "Body";

		private Rectangle _horizontalSelectedColor;
		private Rectangle _verticalSelectedColor;

		private const string HorizontalSelectedColorName = "HorizontalSelectedColor";
		private const string VerticalSelectedColorName = "VerticalSelectedColor";

		private SuperSlider _horizontalSlider;
		private SuperSlider _verticalSlider;

		//		private SuperSliderUpdate _horizontalSlider;
		private const string HorizontalSliderName = "HorizontalSlider";
		private const string VerticalSliderName = "VerticalSlider";

		private const string HorizontalTemplateName = "HorizontalTemplate";
		private const string VerticalTemplateName = "VerticalTemplate";

		#endregion

		public ColorSlider2()
		{
			DefaultStyleKey = typeof(ColorSlider2);

			IsEnabledChanged += SuperSlider_IsEnabledChanged;
			SizeChanged += UserControl_SizeChanged;
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			Body = GetTemplateChild(BodyName) as Grid;

			_horizontalSlider = GetTemplateChild(HorizontalSliderName) as SuperSlider;
			_verticalSlider = GetTemplateChild(VerticalSliderName) as SuperSlider;

			_horizontalSelectedColor = GetTemplateChild(HorizontalSelectedColorName) as Rectangle;
			_verticalSelectedColor = GetTemplateChild(VerticalSelectedColorName) as Rectangle;

			if (_horizontalSlider != null)
				_horizontalSlider.ApplyTemplate();

			if (_verticalSlider != null)
				_verticalSlider.ApplyTemplate();

			if (Color.A == 0 && Color.R == 0 && Color.G == 0 && Color.B == 0)
				Color = Windows.UI.Color.FromArgb(255, 6, 255, 0); // this should be theme accent brush I think.
			else
				UpdateLayoutBasedOnColor();

			if (Thumb == null)
				Thumb = new ColorSliderThumb();

			IsEnabledVisualStateUpdate();
		}

		void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
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
				DefaultSize = IsVertical() ? ActualWidth : ActualHeight;
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
			DependencyProperty.Register("Thumb", typeof(object), typeof(ColorSlider2), new PropertyMetadata(null, OnThumbChanged));

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

		private static void OnThumbChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as ColorSlider2;

			if (sender != null && e.NewValue != e.OldValue)
				sender.UpdateThumb();
		}

		private void UpdateThumb()
		{
			var slider = IsVertical() ? _verticalSlider : _horizontalSlider;

			// TODO: Rewire in when Thumb is added in

			//if (Thumb != null && slider != null)
			//	slider.Thumb = Thumb;
		}

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

		private void LayoutWiring(bool isVisible, SuperSlider slider, FrameworkElement colorElement, string templateName)
		{
			if (slider == null)
				return;

			slider.ValueChanged -= Slider_ValueChanged;
			var template = GetTemplateChild(templateName) as FrameworkElement;

			if (template != null)
				template.Visibility = (isVisible) ? Visibility.Visible : Visibility.Collapsed;

			if (isVisible)
				slider.ValueChanged += Slider_ValueChanged;

			if (colorElement != null)
				colorElement.Visibility = (IsColorVisible) ? Visibility.Visible : Visibility.Collapsed;
		}

		private void AdjustLayoutBasedOnOrientation()
		{
			var isVert = IsVertical();

			var thumb = Thumb as FrameworkElement;

			if (thumb != null)
			{
				thumb.Height = isVert ? HueSelectorSize : double.NaN;
				thumb.Width = isVert ? double.NaN : HueSelectorSize;
			}

			LayoutWiring(!isVert, _horizontalSlider, _horizontalSelectedColor, HorizontalTemplateName);
			LayoutWiring(isVert, _verticalSlider, _verticalSelectedColor, VerticalTemplateName);
		}

		protected internal override void UpdateLayoutBasedOnColor()
		{
			if (_fromSliderChange)
				return;

			base.UpdateLayoutBasedOnColor();

			var colorHue = Color.GetHue();

			if (_horizontalSlider != null)
				_horizontalSlider.Value = colorHue;

			if (_verticalSlider != null)
				_verticalSlider.Value = colorHue;
		}

		private void IsEnabledVisualStateUpdate()
		{
			VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);

			var colorBrush = IsEnabled ? ColorSpace.GetColorGradientBrush(Orientation) : ColorSpace.GetBlackAndWhiteGradientBrush(Orientation);

			if (_horizontalSlider != null)
				_horizontalSlider.Background = colorBrush;

			if (_verticalSlider != null)
				_verticalSlider.Background = colorBrush;
		}


		private bool IsVertical()
		{
			return (Orientation == Orientation.Vertical);
		}
	}
}
