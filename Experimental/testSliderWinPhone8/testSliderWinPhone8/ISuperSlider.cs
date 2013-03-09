using System.Windows.Controls;

namespace testSliderWinPhone8
{
	interface ISuperSlider
	{
		Orientation Orientation { get; set; }
		object Thumb { get; set; }

		double StepFrequency { get; set; }
		double Minimum { get; set; }
		double Maximum { get; set; }
		double Value { get; set; }

		double ProgressSize { get; set; }
		double BackgroundSize { get; set; }
		double BarWidth { get; set; }
		double BarHeight { get; set; }

		string Title { get; set; }
	}
}
