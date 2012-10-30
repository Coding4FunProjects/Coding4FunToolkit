using System;

namespace Coding4Fun.Toolkit.Controls
{
	public class MinuteTimeSpanDataSource : TimeSpanDataSource
	{
		public MinuteTimeSpanDataSource() : base(59, 1) { }

		public MinuteTimeSpanDataSource(int max, int step) : base(max, step) { }

		protected override TimeSpan? GetRelativeTo(TimeSpan relativeDate, int delta)
		{
			return new TimeSpan(relativeDate.Hours, ComputeRelativeTo(relativeDate.Minutes, delta), relativeDate.Seconds);
		}
	}
}
