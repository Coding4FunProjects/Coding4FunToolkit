using System;

namespace Coding4Fun.Toolkit.Controls
{
	public class HourTimeSpanDataSource : TimeSpanDataSource
	{
		public HourTimeSpanDataSource() : base(23, 1) { }

		public HourTimeSpanDataSource(int max, int step) : base(max, step) { }

		protected override TimeSpan? GetRelativeTo(TimeSpan relativeDate, int delta)
		{
			return new TimeSpan(ComputeRelativeTo(relativeDate.Hours, delta), relativeDate.Minutes, relativeDate.Seconds);
		}

	}
}
