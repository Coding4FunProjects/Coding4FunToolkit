using System;

using Coding4Fun.Toolkit.Controls.Common;

namespace Coding4Fun.Toolkit.Controls.Primitives
{
    /// <summary>
    /// TimeSpanPickerBasePage : For easier instanciation in xaml page
    /// </summary>
    public abstract class TimeSpanPickerBasePage : ValuePickerBasePage<TimeSpan>, ITimeSpanPickerPage<TimeSpan>
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
		/// Maximum value as a TimeSpan
        /// </summary>
		public TimeSpan Maximum
        {
            get;
            set;
        }

        /// <summary>
        /// Min value as a TimeSpan
        /// </summary>
		public TimeSpan Minimum
        {
            get;
            set;
        }

		/// <summary>
		/// Value step as a TimeSpan
		/// </summary>
		public TimeSpan StepFrequency
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
                if (value.HasValue)
                {
					base.Value = value.Value.CheckBound(Minimum, Maximum);
                }                
            }
        }
    }
}