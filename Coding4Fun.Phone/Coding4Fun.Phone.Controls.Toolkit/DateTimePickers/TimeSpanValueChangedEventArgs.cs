using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Coding4Fun.Phone.Controls.Toolkit
{
    public class TimeSpanValueChangedEventArgs : ValueChangedEventArgs<TimeSpan>
    {
        /// <summary>
        /// Initializes a new instance of the DateTimeValueChangedEventArgs class.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        public TimeSpanValueChangedEventArgs(TimeSpan? oldTimeSpanValue, TimeSpan? newTimeSpanValue) 
            : base(oldTimeSpanValue, newTimeSpanValue)
        {
        }
    }
}
