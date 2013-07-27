using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Toolkit.Controls
{
	// WP 8 Version
	public class SuperSlider : Slider, ISuperSlider
	{
		public SuperSlider()
		{
			DefaultStyleKey = typeof(SuperSlider);
		}


		public object Thumb { get; set; }
		public double StepFrequency { get; set; }
		public double TrackSize { get; set; }
		public double FillSize { get; set; }
		public string Title { get; set; }
		public Thickness HorizontalPadding { get; set; }
		public Thickness VerticalPadding { get; set; }
		public double DisableTrackOpacity { get; set; }
	}
}
