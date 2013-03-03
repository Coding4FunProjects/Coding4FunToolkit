using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace testSliderWinPhone8
{
	public class SuperSlider : Slider
	{
		public SuperSlider()
		{
			DefaultStyleKey = typeof(SuperSlider);

			ValueChanged += SuperSlider_ValueChanged;
		}

		void SuperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var slide = sender as SuperSlider;

			if (slide == null)
				return;

			if (slide.Value % slide.StepFrequency < slide.StepFrequency / 2.0)
			{
				slide.Value = Math.Floor((slide.Value / slide.StepFrequency)) * slide.StepFrequency;
			}
			else
			{
				slide.Value = Math.Ceiling((slide.Value / slide.StepFrequency)) * slide.StepFrequency;
			}
		}

		public double StepFrequency
		{
			get { return (double)GetValue(StepFrequencyProperty); }
			set { SetValue(StepFrequencyProperty, value); }
		}

		// Using a DependencyProperty as the backing store for StepFrequency.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StepFrequencyProperty =
			DependencyProperty.Register("StepFrequency", typeof(double), typeof(SuperSlider), new PropertyMetadata(1d));


	}
}
