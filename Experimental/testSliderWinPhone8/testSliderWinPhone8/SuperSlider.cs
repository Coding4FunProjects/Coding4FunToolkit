using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace testSliderWinPhone8
{
	public class SuperSlider : Slider, ISuperSlider
	{
		private const string HorizontalRectClipName = "HorizontalRectClip";
		private const string VerticalRectClipName = "VerticalRectClip";

		public SuperSlider()
		{
			DefaultStyleKey = typeof(SuperSlider);
		}

		void SuperSlider_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var horRectClip = GetTemplateChild(HorizontalRectClipName) as RectangleGeometry;

			if (horRectClip != null)
				horRectClip.Rect = new Rect(0, 0, horRectClip.Rect.Width, ActualHeight);

			var vertRectClip = GetTemplateChild(VerticalRectClipName) as RectangleGeometry;

			if (vertRectClip != null)
				vertRectClip.Rect = new Rect(0, vertRectClip.Rect.Y, ActualWidth, vertRectClip.Rect.Height);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			SizeChanged += SuperSlider_SizeChanged;
			ValueChanged += SuperSlider_ValueChanged;
	
			//Dispatcher.BeginInvoke(() =>
			{
				UpdateThumb();
				UpdateValue(Value);
			}
			//);
		}

		void SuperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var slide = sender as SuperSlider;

			if (slide == null)
				return;

			slide.UpdateValue(e.NewValue);
		}

		public void UpdateValue(double newValue)
		{
			if (newValue % StepFrequency < StepFrequency / 2.0)
			{
				Value = Math.Floor((newValue / StepFrequency)) * StepFrequency;
			}
			else
			{
				Value = Math.Ceiling((newValue / StepFrequency)) * StepFrequency;
			}
		}

		public string Title { get; set; }

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

		public double TrackSize
		{
			get { return (double)GetValue(TrackSizeProperty); }
			set { SetValue(TrackSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for TrackSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TrackSizeProperty =
			DependencyProperty.Register("TrackSize", typeof(double), typeof(SuperSlider), new PropertyMetadata(double.NaN));

		public double FillSize
		{
			get { return (double)GetValue(FillSizeProperty); }
			set { SetValue(FillSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for FillSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty FillSizeProperty =
			DependencyProperty.Register("FillSize", typeof(double), typeof(SuperSlider), new PropertyMetadata(double.NaN));

		public double StepFrequency
		{
			get { return (double)GetValue(StepFrequencyProperty); }
			set { SetValue(StepFrequencyProperty, value); }
		}

		// Using a DependencyProperty as the backing store for StepFrequency.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StepFrequencyProperty =
			DependencyProperty.Register("StepFrequency", typeof(double), typeof(SuperSlider), new PropertyMetadata(1d));

		public object Thumb
		{
			get { return (object)GetValue(ThumbProperty); }
			set { SetValue(ThumbProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Thumb.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ThumbProperty =
			DependencyProperty.Register("Thumb", typeof(object), typeof(SuperSlider), new PropertyMetadata(OnThumbChanged));

		private static void OnThumbChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as SuperSlider;

			if (sender != null && e.NewValue != e.OldValue)
				sender.UpdateThumb();
		}

		private void UpdateThumb()
		{
			var thumbItem = GetTemplateChild(IsVertical() ? "VerticalCenterElement" : "HorizontalCenterElement") as ContentPresenter;

			if (Thumb != null && thumbItem != null)
				thumbItem.Content = Thumb;
		}

		private bool IsVertical()
		{
			return (Orientation == Orientation.Vertical);
		}
	}
}
