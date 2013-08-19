using System.Windows.Controls;

using Coding4Fun.Toolkit.Controls;
using Coding4Fun.Toolkit.Controls.Common;

namespace TestClip
{
	public partial class TestSlider
	{
		private MovementMonitor _monitor;
		private const string HorizontalTemplateName = "HorizontalTemplate";

		private void ApplyPlatformFeatures()
		{
			var body = GetTemplateChild(HorizontalTemplateName) as Grid;

			if (body != null)
			{
				_monitor = new MovementMonitor();
				_monitor.Movement += _monitor_Movement;
				_monitor.MonitorControl(body);
			}
		}

		private void _monitor_Movement(object sender, MovementMonitorEventArgs e)
		{
			UpdateSampleBasedOnManipulation(e.X, e.Y);
		}

		private void UpdateSampleBasedOnManipulation(double x, double y)
		{
			var controlMax = GetControlMax();

			var offsetValue = (IsVertical()) ? controlMax - y : x;
			var controlDist = ControlHelper.CheckBound(offsetValue, controlMax);

			var calculateValue = Minimum;

			if (controlMax != 0)
				calculateValue += (Maximum - Minimum) * (controlDist / controlMax);

			SyncValueAndPosition(calculateValue, Value);
		}

		private double GetControlMax()
		{
			return (IsVertical()) ? ActualHeight : ActualWidth;
		}

	}
}
