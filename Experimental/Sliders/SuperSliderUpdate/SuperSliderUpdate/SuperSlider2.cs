using Coding4Fun.Toolkit.Controls;
using Coding4Fun.Toolkit.Controls.Binding;
using Coding4Fun.Toolkit.Controls.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SuperSliderUpdate
{
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    //#if WP7
    public
        //#else
        //	internal 
        //#endif
        class SuperSlider2 : Control
    {
        bool _isLayoutInit;
        public event RoutedPropertyChangedEventHandler<double> ValueChanged;

        protected Rectangle BackgroundRectangle;
        private const string BackgroundRectangleName = "BackgroundRectangle";

        protected Rectangle ProgressRectangle;
        private const string ProgressRectangleName = "ProgressRectangle";

        private ContentPresenter SliderThumb;

        private MovementMonitor _monitor;
        private const string BodyName = "Body";
        private const string BarBodyName = "BarBody";
        private const string ThumbName = "Thumb";

        public SuperSlider2()
        {
            DefaultStyleKey = typeof(SuperSlider2);

            PreventScrollBinding.SetIsEnabled(this, true);

            IsEnabledChanged += SuperSlider_IsEnabledChanged;
            Loaded += SuperSlider_Loaded;
            SizeChanged += SuperSlider_SizeChanged;
        }

        void SuperSlider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsEnabledVisualStateUpdate();
        }

        void SuperSlider_Loaded(object sender, RoutedEventArgs e)
        {
            _isLayoutInit = true;
            AdjustAndUpdateLayout();
            IsEnabledVisualStateUpdate();
        }

        void SuperSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustAndUpdateLayout();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BackgroundRectangle = GetTemplateChild(BackgroundRectangleName) as Rectangle;
            ProgressRectangle = GetTemplateChild(ProgressRectangleName) as Rectangle;
            var body = GetTemplateChild(BodyName) as Grid;

            this.SliderThumb = GetTemplateChild(ThumbName) as ContentPresenter;

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

        public double BarHeight
        {
            get { return (double)GetValue(BarHeightProperty); }
            set { SetValue(BarHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BarHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BarHeightProperty =
            DependencyProperty.Register("BarHeight", typeof(double), typeof(SuperSlider2), new PropertyMetadata(24d, OnLayoutChanged));

        public double BarWidth
        {
            get { return (double)GetValue(BarWidthProperty); }
            set { SetValue(BarWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BarHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BarWidthProperty =
            DependencyProperty.Register("BarWidth", typeof(double), typeof(SuperSlider2), new PropertyMetadata(24d, OnLayoutChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SuperSlider2), new PropertyMetadata(""));


        public DataTemplate ThumbTemplate
        {
            get { return (DataTemplate)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbProperty =
            DependencyProperty.Register("ThumbTemplate", typeof(DataTemplate), typeof(SuperSlider2), new PropertyMetadata(OnLayoutChanged));

        public double BackgroundSize
        {
            get { return (double)GetValue(BackgroundSizeProperty); }
            set { SetValue(BackgroundSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundSizeProperty =
            DependencyProperty.Register("BackgroundSize", typeof(double), typeof(SuperSlider2), new PropertyMetadata(double.NaN, OnLayoutChanged));

        public double ProgressSize
        {
            get { return (double)GetValue(ProgressSizeProperty); }
            set { SetValue(ProgressSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressSizeProperty =
            DependencyProperty.Register("ProgressSize", typeof(double), typeof(SuperSlider2), new PropertyMetadata(double.NaN, OnLayoutChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(SuperSlider2), new PropertyMetadata(0d, OnValueChanged));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(SuperSlider2), new PropertyMetadata(0d));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(SuperSlider2), new PropertyMetadata(10d));

        public double StepFrequency
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StepFrequency.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("StepFrequency", typeof(double), typeof(SuperSlider2), new PropertyMetadata(0d));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SuperSlider2), new PropertyMetadata(Orientation.Horizontal, OnLayoutChanged));

        #endregion

        void _monitor_Movement(object sender, MovementMonitorEventArgs e)
        {
            UpdateSampleBasedOnManipulation(e.X, e.Y);
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as SuperSlider2;

            if (sender != null && e.NewValue != e.OldValue)
                sender.SyncValueAndPosition((double)e.NewValue, (double)e.OldValue);
        }

        private static void OnLayoutChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as SuperSlider2;

            if (sender != null && e.NewValue != e.OldValue)
                sender.AdjustAndUpdateLayout();
        }

        private void UpdateSampleBasedOnManipulation(double x, double y)
        {
            var controlMax = GetControlMax();

            var offsetValue = (IsVertical()) ? controlMax - y : x;
            var controlDist = ControlHelper.CheckBound(offsetValue, controlMax);

            var calculateValue = Minimum;

            if (controlMax != 0)
                calculateValue += (Maximum - Minimum) * (controlDist / controlMax);

            SyncValueAndPosition(calculateValue, Value);
        }

        private double GetControlMax()
        {
            return (IsVertical()) ? ActualHeight : ActualWidth;
        }

        private void SyncValueAndPosition(double newValue, double oldValue)
        {
            if (!_isLayoutInit)
                return;

            _isLayoutInit = true;

            if (StepFrequency > 0)
            {
                var stepDiff = (newValue % StepFrequency);
                var floor = Math.Floor(newValue - stepDiff);

                newValue = stepDiff < (StepFrequency / 2d) ? floor : floor + StepFrequency;
            }

            newValue = ControlHelper.CheckBound(newValue, Minimum, Maximum);

            if (oldValue == newValue)
                return;

            Value = newValue;

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

            if (this.SliderThumb != null)
            {
                var thumbItemSize = (isVert ? SliderThumb.ActualHeight : SliderThumb.ActualWidth);
                var marginOffset = ControlHelper.CheckBound(offset - (thumbItemSize / 2d), 0, controlMax - thumbItemSize);

                SliderThumb.Margin = isVert ? new Thickness(0, 0, 0, marginOffset) : new Thickness(marginOffset, 0, 0, 0);
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
            var bar = GetTemplateChild(BarBodyName) as Grid;

            if (bar != null)
            {
                if (isVert)
                {
                    //var widthBinding = new System.Windows.Data.Binding
                    //{
                    //    Source = bar,
                    //    Path = new PropertyPath("Width"),
                    //    Mode = BindingMode.TwoWay
                    //};

                    //SetBinding(BarWidthProperty, widthBinding);

                    bar.Width = BarWidth;
                    bar.Height = double.NaN;
                }
                else
                {
                    //var heightBinding = new System.Windows.Data.Binding
                    //{
                    //    Source = bar,
                    //    Path = new PropertyPath("Height"),
                    //    Mode = BindingMode.TwoWay
                    //};

                    //SetBinding(BarHeightProperty, heightBinding);

                    bar.Width = double.NaN;
                    bar.Height = BarHeight;
                }
            }


            SetAlignment(ProgressRectangle, isVert);

            ProgressRectangle.Width = double.NaN;
            ProgressRectangle.Height = double.NaN;
            BackgroundRectangle.Width = double.NaN;
            BackgroundRectangle.Height = double.NaN;

            if (ProgressSize > 0)
                SetSizeBasedOnOrientation(ProgressRectangle, !isVert, ProgressSize);

            if (BackgroundSize > 0)
                SetSizeBasedOnOrientation(BackgroundRectangle, !isVert, BackgroundSize);

            if (this.SliderThumb != null && this.ThumbTemplate != null)
            {
                this.SliderThumb.ContentTemplate = this.ThumbTemplate;
                SetAlignment(this.SliderThumb, isVert);
            }
        }

        #region Helper functions

        private void IsEnabledVisualStateUpdate()
        {
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
        }

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
