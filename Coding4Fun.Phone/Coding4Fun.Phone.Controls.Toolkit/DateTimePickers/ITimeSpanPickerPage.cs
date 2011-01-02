namespace Coding4Fun.Phone.Controls.Primitives
{
    public interface ITimeSpanPickerPage<T> : IValuePickerPage<T> where T : struct
    {

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
