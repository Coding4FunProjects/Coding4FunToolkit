using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace App1
{
	public class SuperSlider : Slider
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
		}

		void SuperSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
		{
			if (Orientation == Orientation.Horizontal)
			{
				if (_horizontalThumb == null || _horizontalDecreaseRect == null)
					return;

				_horizontalDecreaseRect.Width = CalculateRectangleSize(ActualWidth, _horizontalThumb.Width, e.NewValue);
			}
			else
			{
				if (_verticalThumb == null || _verticalDecreaseRect == null)
					return;

				_verticalDecreaseRect.Height = CalculateRectangleSize(ActualHeight, _verticalThumb.Width, e.NewValue);
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
	}
}
