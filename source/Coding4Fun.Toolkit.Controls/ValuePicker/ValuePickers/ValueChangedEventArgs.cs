using System;

namespace Coding4Fun.Toolkit.Controls
{
    /// <summary>
    /// Provides data for the DatePicker and TimePicker's ValueChanged event.
    /// </summary>
    public class ValueChangedEventArgs<T> : EventArgs where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the DateTimeValueChangedEventArgs class.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        public ValueChangedEventArgs(T? oldValue, T? newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets or sets the old DateTime value.
        /// </summary>
        public T? OldValue { get; private set; }

        /// <summary>
        /// Gets or sets the new DateTime value.
        /// </summary>
        public T? NewValue { get; private set; }
    }
}
