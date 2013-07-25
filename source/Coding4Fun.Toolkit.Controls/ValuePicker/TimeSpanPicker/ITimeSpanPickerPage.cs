namespace Coding4Fun.Toolkit.Controls.Primitives
{
    public interface ITimeSpanPickerPage<T> : IValuePickerPage<T> where T : struct
    {
		//Value is in IValuePickerPage base

        /// <summary>
        /// The minimum of Value
        /// </summary>
        T Min { get; set; }

        /// <summary>
        /// The maximum of Value
        /// </summary>
        T Max { get; set; }

        /// <summary>
        /// The step for going from value to next value or previous value
        /// </summary>
        T IncrementStep { get; set; }
    }
}
