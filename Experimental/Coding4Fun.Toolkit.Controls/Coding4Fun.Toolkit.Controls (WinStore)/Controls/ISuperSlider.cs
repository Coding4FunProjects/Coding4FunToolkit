using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coding4Fun.Toolkit.Controls
{
	internal interface ISuperSlider
	{
		Orientation Orientation { get; set; }

		double StepFrequency { get; set; }
		double Minimum { get; set; }
		double Maximum { get; set; }
		double Value { get; set; }

		double TrackSize { get; set; }
		double FillSize { get; set; }

		string Title { get; set; }

		Thickness HorizontalPadding { get; set; }
		Thickness VerticalPadding { get; set; }

		double DisableTrackOpacity { get; set; }
	}
}
