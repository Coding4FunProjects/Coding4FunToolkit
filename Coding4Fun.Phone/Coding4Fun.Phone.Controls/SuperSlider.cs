using System;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;
using System.Windows.Shapes;
using Coding4Fun.Phone.Controls.Helpers;

namespace Coding4Fun.Phone.Controls
{
    public class SuperSlider : Control
    {
        bool _isLayoutInit;
        public event RoutedPropertyChangedEventHandler<double> ValueChanged;

        protected Rectangle BackgroundRectangle;
        private const string BackgroundRectangleName = "BackgroundRectangle";

        protected Rectangle ProgressRectangle;
        private const string ProgressRectangleName = "ProgressRectangle";

        private MovementMonitor _monitor;
        private const string BodyName = "Body";

        public SuperSlider()
		{
            DefaultStyleKey = typeof(SuperSlider);
            
            Loaded += SuperSlider_Loaded;
		}

        void SuperSlider_Loaded(object sender, RoutedEventArgs e)
        {
            _isLayoutInit = true;
            AdjustAndUpdateLayout();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BackgroundRectangle = GetTemplateChild(BackgroundRectangleName) as Rectangle;
            ProgressRectangle = GetTemplateChild(ProgressRectangleName) as Rectangle;
            var body = GetTemplateChild(BodyName) as Grid;

            if (body != null)
            {
                _monitor = new MovementMonitor();
                _monitor.Movement += _monitor_Movement;
                _monitor.MonitorControl(body);
            }

            // stuff isn't set enough but if this isn't done, there will an initial flash
            AdjustLayout();
        }

        #region dependency properties
        public object Thumb
        {
            get { return (object)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbProperty =
            DependencyProperty.Register("Thumb", typeof(object), typeof(SuperSlider), new PropertyMetadata(OnLayoutChanged));

        public double BackgroundSize
        {
            get { return (double)GetValue(BackgroundSizeProperty); }
            set { SetValue(BackgroundSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundSizeProperty =
            DependencyProperty.Register("BackgroundSize", typeof(double), typeof(SuperSlider), new PropertyMetadata(double.NaN, OnLayoutChanged));

        public double ProgressSize
        {
            get { return (double)GetValue(ProgressSizeProperty); }
            set { SetValue(ProgressSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressSizeProperty =
            DependencyProperty.Register("ProgressSize", typeof(double), typeof(SuperSlider), new PropertyMetadata(double.NaN, OnLayoutChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(SuperSlider), new PropertyMetadata(0d, OnValueChanged));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(SuperSlider), new PropertyMetadata(0d));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(SuperSlider), new PropertyMetadata(10d));

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(SuperSlider), new PropertyMetadata(0d));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SuperSlider), new PropertyMetadata(Orientation.Horizontal, OnLayoutChanged));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(SuperSlider), new PropertyMetadata(null));
        #endregion

        void _monitor_Movement(object sender, MovementMonitorEventArgs e)
        {
            UpdateSampleBasedOnManipulation(e.X, e.Y);
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as SuperSlider;

            if (sender != null && e.NewValue != e.OldValue)
                sender.UpdateSampleBasedOnValue(sender.Value);
        }

        private static void OnLayoutChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as SuperSlider;

            if (sender != null && e.NewValue != e.OldValue)
                sender.AdjustAndUpdateLayout();
        }

        private void UpdateSampleBasedOnManipulation(double x, double y)
        {
            var controlMax = GetControlMax();

            var offsetValue = (IsVertical()) ? controlMax - y : x;
            var controlDist = ControlHelper.CheckBound(offsetValue, controlMax);

            var calculateValue = Minimum;

            if(controlMax != 0)
                calculateValue += (Maximum - Minimum) * (controlDist / controlMax);

            SyncValueAndPosition(calculateValue);
        }

        private void UpdateSampleBasedOnValue(double value)
        {
            SyncValueAndPosition(value);
        }

        private double GetControlMax()
        {
            return (IsVertical()) ? ActualHeight : ActualWidth;
        }

        private void SyncValueAndPosition(double value)
        {
            if (!_isLayoutInit)
                return;

            _isLayoutInit = true;

            var oldValue = Value;

            if (Step > 0)
            {
                var stepDiff = (value % Step);
                var floor = Math.Floor(value - stepDiff);

                value = stepDiff < (Step / 2d) ? floor : floor + Step;
            }

            value = ControlHelper.CheckBound(value, Minimum, Maximum);

            if (oldValue == value)
                return;

            Value = value;

            UpdateUserInterface();

            if (ValueChanged != null)
                ValueChanged(this, new RoutedPropertyChangedEventArgs<double>(oldValue, Value));
        }

        private void UpdateUserInterface()
        {
            var controlMax = GetControlMax();
            var offset = ((Value - Minimum) / (Maximum - Minimum)) * controlMax;

            var isVert = IsVertical();

            SetSizeBasedOnOrientation(ProgressRectangle, isVert, offset);

            var thumbItem = Thumb as FrameworkElement;

            if (thumbItem != null)
            {
                var thumbItemSize = (isVert ? thumbItem.ActualHeight : thumbItem.ActualWidth);
                var marginOffset = ControlHelper.CheckBound(offset - (thumbItemSize / 2d), 0, controlMax - thumbItemSize);

                thumbItem.Margin = isVert ? new Thickness(0, 0, 0, marginOffset) : new Thickness(marginOffset, 0, 0, 0);
            }
        }

        private void AdjustAndUpdateLayout()
        {
            AdjustLayout();
            UpdateUserInterface();
        }

        private void AdjustLayout()
        {
            if (ProgressRectangle == null || BackgroundRectangle == null)
                return;

            var isVert = IsVertical();

            SetAlignment(ProgressRectangle, isVert);

            ProgressRectangle.Width = double.NaN;
            ProgressRectangle.Height = double.NaN;
            BackgroundRectangle.Width = double.NaN;
            BackgroundRectangle.Height = double.NaN;

            if (ProgressSize > 0)
                SetSizeBasedOnOrientation(ProgressRectangle, !isVert, ProgressSize);

            if (BackgroundSize > 0)
                SetSizeBasedOnOrientation(BackgroundRectangle, !isVert, BackgroundSize);

            if (Thumb != null)
            {
                SetAlignment(Thumb as FrameworkElement, isVert);
            }
        }

        #region Helper functions

        

        private static void SetSizeBasedOnOrientation(FrameworkElement control, bool isVert, double offset)
        {
            if (control == null)
                return;

            if (isVert)
                control.Height = offset;
            else
                control.Width = offset;
        }

        private bool IsVertical()
        {
            return (Orientation == Orientation.Vertical);
        }

        private static void SetAlignment(FrameworkElement control, bool isVert)
        {
            if (control == null)
                return;

            control.HorizontalAlignment = isVert ? HorizontalAlignment.Stretch : HorizontalAlignment.Left;
            control.VerticalAlignment = isVert ? VerticalAlignment.Bottom : VerticalAlignment.Stretch;
        }
        #endregion
    }
}
