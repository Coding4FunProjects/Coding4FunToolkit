using System;

namespace Coding4Fun.Toolkit.Controls
{
	public class SecondTimeSpanDataSource : TimeSpanDataSource
	{
		public SecondTimeSpanDataSource() : base(59, 1) { }

		public SecondTimeSpanDataSource(int max, int step) : base(max, step) { }

		protected override TimeSpan? GetRelativeTo(TimeSpan relativeDate, int delta)
		{
			return new TimeSpan(relativeDate.Hours, relativeDate.Minutes, ComputeRelativeTo(relativeDate.Seconds, delta));
		}
	}
}
