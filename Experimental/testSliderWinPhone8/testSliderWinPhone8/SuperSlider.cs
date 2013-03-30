using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace testSliderWinPhone8
{
	public class SuperSlider : Slider, ISuperSlider
	{
		public SuperSlider()
		{
			DefaultStyleKey = typeof(SuperSlider);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

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

		public double TrackSize
		{
			get { return (double)GetValue(TrackSizeProperty); }
			set { SetValue(TrackSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for TrackSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TrackSizeProperty =
			DependencyProperty.Register("TrackSize", typeof(double), typeof(SuperSlider), new PropertyMetadata(12d));

		public double FillSize
		{
			get { return (double)GetValue(FillSizeProperty); }
			set { SetValue(FillSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for FillSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty FillSizeProperty =
			DependencyProperty.Register("FillSize", typeof(double), typeof(SuperSlider), new PropertyMetadata(12d));

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
			var thumbItem = GetTemplateChild(IsVertical() ? "VerticalCenterElement" : "HorizontalCenterElement") as ContentControl;

			if (Thumb != null && thumbItem != null)
				thumbItem.Content = Thumb;
		}

		private bool IsVertical()
		{
			return (Orientation == Orientation.Vertical);
		}
	}
}
