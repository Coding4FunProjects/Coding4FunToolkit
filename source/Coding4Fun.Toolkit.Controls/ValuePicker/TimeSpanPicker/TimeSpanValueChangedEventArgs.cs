using System;

namespace Coding4Fun.Toolkit.Controls
{
    public class TimeSpanValueChangedEventArgs : ValueChangedEventArgs<TimeSpan>
    {
    	/// <summary>
    	/// Initializes a new instance of the DateTimeValueChangedEventArgs class.
    	/// </summary>
		/// <param name="oldTimeSpanValue">Old value.</param>
		/// <param name="newTimeSpanValue">New value.</param>
    	public TimeSpanValueChangedEventArgs(TimeSpan? oldTimeSpanValue, TimeSpan? newTimeSpanValue) 
            : base(oldTimeSpanValue, newTimeSpanValue)
        {
        }
    }
}
