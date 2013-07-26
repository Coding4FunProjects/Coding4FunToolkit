namespace Coding4Fun.Toolkit.Controls.Primitives
{
    public interface ITimeSpanPickerPage<T> : IValuePickerPage<T> where T : struct
    {
		//Value is in IValuePickerPage base

        /// <summary>
        /// The minimum of Value
        /// </summary>
		T Minimum { get; set; }

        /// <summary>
        /// The maximum of Value
        /// </summary>
		T Maximum { get; set; }

        /// <summary>
        /// The step for going from value to next value or previous value
        /// </summary>
		T StepFrequency { get; set; }
    }
}
