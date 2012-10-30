// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Globalization;

namespace Coding4Fun.Toolkit.Controls.Primitives
{
    /// <summary>
    /// Implements a wrapper for TimeSpan that provides formatted strings for TimeSpanPicker.
    /// </summary>
    public class TimeSpanWrapper : ValueWrapper<TimeSpan>
    {
        /// <summary>
        /// Instanciates a derived instance of ValueWrapper
        /// </summary>
        /// <param name="value">the initial value</param>
        /// <returns>the new instance</returns>
        public override ValueWrapper<TimeSpan> CreateNew(TimeSpan? value)
        {
            return new TimeSpanWrapper(value.GetValueOrDefault(TimeSpan.FromMinutes(30)));
        }

        /// <summary>
        /// Gets the DateTime being wrapped.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                return Value;
            }
        }

        /// <summary>
        /// Hour part
        /// </summary>
        public string HourNumber
        {
            get
            {
                return TimeSpan.Hours.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Minute Part
        /// </summary>
        public string MinuteNumber
        {
            get
            {
                return TimeSpan.Minutes.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Second Part
        /// </summary>
        public string SecondNumber
        {
            get
            {
                return TimeSpan.Seconds.ToString(CultureInfo.InvariantCulture);
            }
        }


        /// <summary>
        /// Initializes a new instance of the TimespanWrapper class.
        /// </summary>
        /// <param name="timeSpan">TimeSpan to wrap.</param>
        public TimeSpanWrapper(TimeSpan timeSpan) : base(timeSpan) { }
    }
}
