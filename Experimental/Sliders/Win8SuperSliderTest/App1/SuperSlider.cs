using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace App1
{
	public class SuperSlider : Slider, ISuperSlider
	{
		private const string VerticalThumbName = "VerticalThumb";
		private const string VerticalDecreaseRectName = "Coding4FunVerticalDecreaseRect";

		private const string HorizontalThumbName = "HorizontalThumb";
		private const string HorizontalDecreaseRectName = "Coding4FunHorizontalDecreaseRect";
		
		private Thumb _verticalThumb;
		private Shape _verticalDecreaseRect;

		private Thumb _horizontalThumb;
		private Shape _horizontalDecreaseRect;

		public SuperSlider()
		{
			DefaultStyleKey = typeof(SuperSlider);

			ValueChanged += SuperSlider_ValueChanged;
			SizeChanged += SuperSlider_SizeChanged;
		}

		void SuperSlider_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			UpdateLayoutBasedOnValue(Value);
		}

		void SuperSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
		{
			UpdateValue(e.NewValue);
		}

		private void UpdateValue(Double value)
		{
			var returnValue = value;
			var modAmount = returnValue%StepFrequency;

			if (modAmount != 0 && value != Maximum)
			{
				returnValue = Math.Floor(returnValue / StepFrequency) * StepFrequency;

				var lastPossibleStep = Math.Floor(Maximum / StepFrequency) * StepFrequency;

				if (value > lastPossibleStep)
				{
					if (Maximum%StepFrequency/2.0 < modAmount)
						returnValue = Maximum;
				}

				Value = returnValue;

				return;
			}

			UpdateLayoutBasedOnValue(value);
		}

		private void UpdateLayoutBasedOnValue(double newValue)
		{
			if (Orientation == Orientation.Horizontal)
			{
				if (_horizontalThumb == null || _horizontalDecreaseRect == null)
					return;

				_horizontalDecreaseRect.Width = CalculateRectangleSize(ActualWidth, _horizontalThumb.ActualWidth, newValue);
			}
			else
			{
				if (_verticalThumb == null || _verticalDecreaseRect == null)
					return;

				_verticalDecreaseRect.Height = CalculateRectangleSize(ActualHeight, _verticalThumb.ActualHeight, newValue);
			}
		}

		private double CalculateRectangleSize(double controlSize, double thumbSize, double value)
		{
			var targetValue = ((controlSize - thumbSize) / Maximum) * value;

			if (targetValue < 0)
				targetValue = 0;

			return targetValue;
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_verticalThumb = GetTemplateChild(VerticalThumbName) as Thumb;
			_verticalDecreaseRect = GetTemplateChild(VerticalDecreaseRectName) as Shape;

			_horizontalThumb = GetTemplateChild(HorizontalThumbName) as Thumb;
			_horizontalDecreaseRect = GetTemplateChild(HorizontalDecreaseRectName) as Shape;
		}

		public double DisableTrackOpacity
		{
			get { return (double)GetValue(DisableTrackOpacityProperty); }
			set { SetValue(DisableTrackOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DisableTrackOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DisableTrackOpacityProperty =
			DependencyProperty.Register("DisableTrackOpacity", typeof(double), typeof(SuperSlider), new PropertyMetadata(0.1));


		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(SuperSlider), new PropertyMetadata(""));

		public Thickness VerticalPadding
		{
			get { return (Thickness)GetValue(VerticalPaddingProperty); }
			set { SetValue(VerticalPaddingProperty, value); }
		}

		// Using a DependencyProperty as the backing store for VerticalPadding.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty VerticalPaddingProperty =
			DependencyProperty.Register("VerticalPadding", typeof(Thickness), typeof(SuperSlider), new PropertyMetadata(new Thickness()));

		public Thickness HorizontalPadding
		{
			get { return (Thickness)GetValue(HorizontalPaddingProperty); }
			set { SetValue(HorizontalPaddingProperty, value); }
		}

		// Using a DependencyProperty as the backing store for HorizontalPadding.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HorizontalPaddingProperty =
			DependencyProperty.Register("HorizontalPadding", typeof(Thickness), typeof(SuperSlider), new PropertyMetadata(new Thickness()));


		public object Thumb
		{
			get { return (object)GetValue(ThumbProperty); }
			set { SetValue(ThumbProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Thumb.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ThumbProperty =
			DependencyProperty.Register("Thumb", typeof(object), typeof(SuperSlider), new PropertyMetadata(null, OnThumbChanged));


		public double TrackSize
		{
			get { return (double)GetValue(TrackSizeProperty); }
			set { SetValue(TrackSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BackgroundSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TrackSizeProperty =
			DependencyProperty.Register("TrackSize", typeof(double), typeof(SuperSlider), new PropertyMetadata(12d, OnLayoutChanged));

		public double FillSize
		{
			get { return (double)GetValue(FillSizeProperty); }
			set { SetValue(FillSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ProgressSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty FillSizeProperty =
			DependencyProperty.Register("FillSize", typeof(double), typeof(SuperSlider), new PropertyMetadata(12d, OnLayoutChanged));

		private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//throw new NotImplementedException();
		}

		private static void OnThumbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// throw new NotImplementedException();
		}

	}
}
