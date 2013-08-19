using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestClip
{
	public partial class TestSlider : Slider
	{
		private const string HorizontalTrackName = "HorizontalTrack";
		private const string HorizontalFillName = "HorizontalFill";
		private const string HorizontalRectangleGeometryName = "HorizontalRectangleGeometry";
		private const string HorizontalThumbTranslateTransformName = "HorizontalThumbTranslateTransform";
		private const string HorizontalThumbName = "HorizontalThumb";

		private Rectangle _horizontalTrack;
		private Rectangle _horizontalFill;
		private RectangleGeometry _horizontalRectangleGeometry;
		private Thumb _horizontalThumb;
		private TranslateTransform _horizontalThumbTranslateTransform;

		private bool _isLayoutInit;

		public double StepFrequency
		{
			get { return (double)GetValue(StepProperty); }
			set { SetValue(StepProperty, value); }
		}

		// Using a DependencyProperty as the backing store for StepFrequency.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StepProperty =
			DependencyProperty.Register("StepFrequency", typeof(double), typeof(TestSlider), new PropertyMetadata(10d));
		

		public TestSlider()
		{
			DefaultStyleKey = typeof (TestSlider);

			Maximum = 110;
			Value = 55;
			StepFrequency = 25;

			Loaded += TestSlider_Loaded;
			ValueChanged += TestSlider_ValueChanged;
		}

		void TestSlider_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateUserInterface();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			ApplyingTemplate();

			_horizontalThumb = GetTemplateChild(HorizontalThumbName) as Thumb;
			_horizontalTrack = GetTemplateChild(HorizontalTrackName) as Rectangle;
			_horizontalFill = GetTemplateChild(HorizontalFillName) as Rectangle;
			_horizontalRectangleGeometry = GetTemplateChild(HorizontalRectangleGeometryName) as RectangleGeometry;
			_horizontalThumbTranslateTransform = GetTemplateChild(HorizontalThumbTranslateTransformName) as TranslateTransform;

			SyncValueAndPosition(Value, Value);

			_isLayoutInit = true;
		}

		public bool IsVertical()
		{
			return false;
		}


		private void SyncValueAndPosition(double newValue, double oldValue)
		{
			var returnValue = newValue;
			var modAmount = returnValue % StepFrequency;

			if (modAmount != 0 && newValue != Maximum)
			{
				returnValue = Math.Round(returnValue / StepFrequency) * StepFrequency;

				var lastPossibleStep = Math.Floor(Maximum / StepFrequency) * StepFrequency;

				if (newValue > lastPossibleStep)
				{
					if (Maximum % StepFrequency / 2.0 < modAmount)
						returnValue = Maximum;
				}

				Value = returnValue;

				return;
			}

			UpdateUserInterface();
		}


		void TestSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			SyncValueAndPosition(e.NewValue, e.OldValue);
		}

		private void UpdateUserInterface()
		{
			if (!_isLayoutInit)
				return;

			var thumbWidth = _horizontalThumb.ActualWidth;
			var trackWidth = _horizontalTrack.ActualWidth;
			var clipValue = (Value / Maximum) * (trackWidth);
			var translateValue = clipValue - thumbWidth / 2d;

			_horizontalThumbTranslateTransform.X = translateValue;
			_horizontalRectangleGeometry.Rect = new Rect(0, 0, clipValue, _horizontalFill.ActualHeight);
		}
	}
}