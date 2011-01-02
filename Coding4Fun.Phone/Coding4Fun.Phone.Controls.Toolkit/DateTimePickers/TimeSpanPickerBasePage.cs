using System;

namespace Coding4Fun.Phone.Controls.Primitives
{
    /// <summary>
    /// TimeSpanPickerBasePage : For easier instanciation in xaml page
    /// </summary>
    public abstract partial class TimeSpanPickerBasePage : ValuePickerBasePage<TimeSpan>, ITimeSpanPickerPage<TimeSpan>
    {

        /// <summary>
        /// Instanciates a TimeSpan Wrapper
        /// </summary>
        /// <param name="value">the initial TimeSpan value</param>
        /// <returns>the new instance</returns>
        protected override ValueWrapper<TimeSpan> GetNewWrapper(TimeSpan? value)
        {
            return new TimeSpanWrapper(value.GetValueOrDefault(TimeSpan.FromMinutes(30)));
        }

        /// <summary>
        /// Max value as a TimeSpan
        /// </summary>
        public TimeSpan Max
        {
            get;
            set;
        }

        /// <summary>
        /// Value step as a TimeSpan
        /// </summary>
        public TimeSpan IncrementStep
        {
            get;
            set;
        }

        /// <summary>
        /// TimeSpan Value
        /// </summary>
        public override TimeSpan? Value
        {
            set
            {
                base.Value = value >= Max ? Max - IncrementStep: value;
            }
        }


    }
}
