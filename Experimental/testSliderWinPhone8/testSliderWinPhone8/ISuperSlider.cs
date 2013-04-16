using System.Windows;
using System.Windows.Controls;

namespace testSliderWinPhone8
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

		string Title { get; set; }

		Thickness HorizontalPadding { get; set; }
		Thickness VerticalPadding { get; set; }
	}
}
