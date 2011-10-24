using System;
using System.Globalization;
using System.Windows;

using Coding4Fun.Phone.Controls.Primitives;
using Coding4Fun.Phone.Controls.Toolkit.Common;

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
			DialogTitle = DateTimePickerResources.TimeSpanPickerTitle;
        }

        protected internal override void UpdateValueString()
        {
            if (Value.HasValue)
            {
                var ts = Value.Value;

                if (Max > TimeSpan.Zero && ts > Max)
                {
                    Value = Max;
                    return;
                }

                if (!string.IsNullOrEmpty(ValueStringFormat))
                {
                    ValueString = TimeSpanFormat.Format(ts, ValueStringFormat);
                    return;
                }
            }

            ValueString = string.Format(CultureInfo.CurrentCulture, ValueStringFormat ?? ValueStringFormatFallback, Value);
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
        /// Event that is invoked when the Value property changes.
        /// </summary>
        public event EventHandler<TimeSpanValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Called when the value changes.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnValueChanged(ValueChangedEventArgs<TimeSpan> e)
        {
            EventHandler<TimeSpanValueChangedEventArgs> handler = ValueChanged;
            if (null != handler)
            {
                handler(this, new TimeSpanValueChangedEventArgs(e.OldValue.GetValueOrDefault(), e.NewValue.GetValueOrDefault()));
            }
        }


        /// <summary>
        /// Initializes Value, Max, Step when vanigating to the new page
        /// </summary>
        /// <param name="page"></param>
        protected override void NavigateToNewPage(object page)
        {
            var tsPage = page as ITimeSpanPickerPage<TimeSpan>;

            if (tsPage != null)
            {
                tsPage.Max = Max;
                tsPage.IncrementStep = Step;
            }

            base.NavigateToNewPage(page);
        }


    }
}
