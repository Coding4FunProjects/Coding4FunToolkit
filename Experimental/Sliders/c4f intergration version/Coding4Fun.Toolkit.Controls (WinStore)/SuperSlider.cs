using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Coding4Fun.Toolkit.Controls
{
	// Windows Store Version
	public class SuperSlider : Slider, ISuperSlider
	{
		public SuperSlider()
		{
			DefaultStyleKey = typeof(SuperSlider);
		}

		public object Thumb { get; set; }
		public double TrackSize { get; set; }
		public double FillSize { get; set; }
		public double DisableTrackOpacity { get; set; }
	}
}
