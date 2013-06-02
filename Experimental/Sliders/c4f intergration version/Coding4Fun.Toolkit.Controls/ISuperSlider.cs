#if WINDOWS_STORE
using Windows.UI.Xaml.Controls;

#elif WINDOWS_PHONE
using System.Windows.Controls;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	internal interface ISuperSlider
	{
		Orientation Orientation { get; set; }
		object Thumb { get; set; }

		double StepFrequency { get; set; }
		double Minimum { get; set; }
		double Maximum { get; set; }
		double Value { get; set; }
		
		double TrackSize { get; set; }
		double FillSize { get; set; }

		double DisableTrackOpacity { get; set; }
	}
}
