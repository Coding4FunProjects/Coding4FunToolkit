using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Toolkit.Controls
{
	public class SuperSlider : Control, ISuperSlider
	{
		public SuperSlider()
		{
			DefaultStyleKey = typeof(SuperSlider);
		}


		public Orientation Orientation { get; set; }
		public object Thumb { get; set; }
		public double StepFrequency { get; set; }
		public double Minimum { get; set; }
		public double Maximum { get; set; }
		public double Value { get; set; }
		public double TrackSize { get; set; }
		public double FillSize { get; set; }
		public string Title { get; set; }
		public Thickness HorizontalPadding { get; set; }
		public Thickness VerticalPadding { get; set; }
		public double DisableTrackOpacity { get; set; }
	}
}
