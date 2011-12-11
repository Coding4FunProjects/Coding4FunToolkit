// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

using Coding4Fun.Phone.Controls.Primitives;
using Coding4Fun.Phone.Controls.Toolkit.Primitives;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.Controls.Toolkit
{
    /// <summary>
    /// Represents a page used by the DatespanPicker control that allows the user to choose a duration (hour/minute/second).
    /// </summary>
    public partial class TimeSpanPickerPage : TimeSpanPickerBasePage
    {
        /// <summary>
        /// Initializes a new instance of the TimespanPickerPage control.
        /// </summary>
        public TimeSpanPickerPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hook up the data sources
        /// </summary>
        public override void InitDataSource()
        {
            var stepSeconds = IncrementStep.Seconds;
            var maxSeconds = Max >= TimeSpan.FromMinutes(1) ? 60 : Math.Min(Max.Seconds + stepSeconds, 60);
            TertiarySelector.DataSource = new SecondTimeSpanDataSource(maxSeconds, stepSeconds);

            var stepMinutes = IncrementStep > TimeSpan.FromMinutes(1) ? IncrementStep.Minutes : 1;
            var maxMinutes = Max >= TimeSpan.FromHours(1) ? 60 : Math.Min(Max.Minutes + stepMinutes, 60);
            SecondarySelector.DataSource = new MinuteTimeSpanDataSource(maxMinutes, stepMinutes);

            var stepHours = IncrementStep > TimeSpan.FromHours(1) ? IncrementStep.Hours : 1;
            var maxHours = Max >= TimeSpan.FromHours(24) ? 24 : Max.Hours + stepHours;
            PrimarySelector.DataSource = new HourTimeSpanDataSource(maxHours, stepHours);

            InitializeValuePickerPage(PrimarySelector, SecondarySelector, TertiarySelector);
        }


        /// <summary>
        /// Gets a sequence of LoopingSelector parts ordered according to culture string for date/time formatting.
        /// </summary>
        /// <returns>LoopingSelectors ordered by culture-specific priority.</returns>
        protected override IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern()
        {
            var selectors = GetSelectorsOrderedByCulturePattern(
                CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.ToUpperInvariant(),
                new char[] { 'H', 'M', 'S' },
                new LoopingSelector[] { PrimarySelector, SecondarySelector, TertiarySelector });

            return selectors.Where(s => !(s.DataSource.IsEmpty));
        }

        /// <summary>
        /// Handles changes to the page's Orientation property.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            if (null == e)
            {
                throw new ArgumentNullException("e");
            }

            base.OnOrientationChanged(e);
            SystemTrayPlaceholder.Visibility = (0 != (PageOrientation.Portrait & e.Orientation)) ?
                Visibility.Visible :
                Visibility.Collapsed;
        }
    }
}
