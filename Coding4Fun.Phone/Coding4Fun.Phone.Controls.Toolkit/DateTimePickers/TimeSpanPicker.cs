using System;
using System.Windows;

using Coding4Fun.Phone.Controls.Primitives;

namespace Coding4Fun.Phone.Controls.Toolkit
{
    public class TimeSpanPicker : DateTimePickerBase<TimeSpan>
    {
        /// <summary>
        /// Initializes a new instance of the TimePicker control.
        /// </summary>
        public TimeSpanPicker()
        {
            DefaultStyleKey = typeof(TimeSpanPicker);
            Value = TimeSpan.FromMinutes(30);
            Max = TimeSpan.FromHours(24);
            Step = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Gets or sets the Max Value
        /// </summary>
        public TimeSpan Max
        {
            get { return (TimeSpan)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        /// <summary>
        /// Identifies the Max Property
        /// </summary>
        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(
            "Max", typeof(TimeSpan), typeof(DateTimePickerBase<TimeSpan>), null);


        /// <summary>
        /// Gets or sets the Value Step
        /// </summary>
        public TimeSpan Step
        {
            get { return (TimeSpan)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        /// <summary>
        /// Identifies the Max Property
        /// </summary>
        public static readonly DependencyProperty StepProperty = DependencyProperty.Register(
            "Step", typeof(TimeSpan), typeof(DateTimePickerBase<TimeSpan>), null);


        /// <summary>
        /// Initializes Value, Max, Step when vanigating to the new page
        /// </summary>
        /// <param name="page"></param>
        protected override void NavigateToNewPage(object page)
        {
            var tsPage = page as ITimeSpanPickerPage<TimeSpan>;
            tsPage.Max = Max;
            tsPage.IncrementStep = Step;

            base.NavigateToNewPage(page);
        }


    }
}
