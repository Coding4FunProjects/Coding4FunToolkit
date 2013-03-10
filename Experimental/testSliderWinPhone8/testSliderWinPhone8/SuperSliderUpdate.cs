using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Coding4Fun.Toolkit.Controls.Common;
using testSliderWinPhone8;

namespace Coding4Fun.Toolkit.Controls
{
	[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Normal", GroupName = "GroupCommon")]
//#if WP7
	public
//#else
//	internal 
//#endif
		class SuperSliderUpdate : Control, ISuperSlider
    {
        bool _isLayoutInit;
        public event RoutedPropertyChangedEventHandler<double> ValueChanged;

        private MovementMonitor _monitor;
        private const string BodyName = "Body";
		private const string HorizontalTemplateName = "HorizontalTemplate";
		private const string VerticalTemplateName = "VerticalTemplate";


		public SuperSliderUpdate()
		{
			DefaultStyleKey = typeof(SuperSliderUpdate);

			IsEnabledChanged += SuperSlider_IsEnabledChanged;
            Loaded += SuperSlider_Loaded;
		}

		void SuperSlider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
		    IsEnabledVisualStateUpdate();
		}

	    void SuperSlider_Loaded(object sender, RoutedEventArgs e)
        {
            _isLayoutInit = true;

			IsEnabledVisualStateUpdate();
        }


		public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var body = GetTemplateChild(BodyName) as Grid;

            if (body != null)
            {
                _monitor = new MovementMonitor();
                _monitor.Movement += _monitor_Movement;
                _monitor.MonitorControl(body);
            }

            // stuff isn't set enough but if this isn't done, there will an initial flash
            //AdjustLayout();

			AdjustAndUpdateLayout();
        }

        #region dependency properties

		public double BarHeight
		{
			get { return (double)GetValue(BarHeightProperty); }
			set { SetValue(BarHeightProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BarHeight.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BarHeightProperty =
			DependencyProperty.Register("BarHeight", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(24d, OnLayoutChanged));

		public double BarWidth
		{
			get { return (double)GetValue(BarWidthProperty); }
			set { SetValue(BarWidthProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BarHeight.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BarWidthProperty =
			DependencyProperty.Register("BarWidth", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(24d, OnLayoutChanged));
		
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(SuperSliderUpdate), new PropertyMetadata(""));


        public object Thumb
        {
            get { return (object)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbProperty =
			DependencyProperty.Register("Thumb", typeof(object), typeof(SuperSliderUpdate), new PropertyMetadata(OnLayoutChanged));

        public double BackgroundSize
        {
            get { return (double)GetValue(BackgroundSizeProperty); }
            set { SetValue(BackgroundSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundSizeProperty =
			DependencyProperty.Register("BackgroundSize", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(double.NaN, OnLayoutChanged));

        public double ProgressSize
        {
            get { return (double)GetValue(ProgressSizeProperty); }
            set { SetValue(ProgressSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressSizeProperty =
			DependencyProperty.Register("ProgressSize", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(double.NaN, OnLayoutChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(0d, OnValueChanged));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(0d));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(10d));

        public double StepFrequency
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StepFrequency.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
			DependencyProperty.Register("StepFrequency", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(0d));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
			DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SuperSliderUpdate), new PropertyMetadata(Orientation.Horizontal, OnLayoutChanged));
        #endregion

        void _monitor_Movement(object sender, MovementMonitorEventArgs e)
        {
            UpdateSampleBasedOnManipulation(e.X, e.Y);
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
			var sender = o as SuperSliderUpdate;

            if (sender != null && e.NewValue != e.OldValue)
                sender.SyncValueAndPosition((double)e.NewValue, (double)e.OldValue);
        }

        private static void OnLayoutChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
			var sender = o as SuperSliderUpdate;

            if (sender != null && e.NewValue != e.OldValue)
                sender.AdjustAndUpdateLayout();
        }

		private void AdjustAndUpdateLayout()
		{
			bool isVert = IsVertical();

			var hor = GetTemplateChild(HorizontalTemplateName) as FrameworkElement;
			var vert = GetTemplateChild(VerticalTemplateName) as FrameworkElement;

			if (hor != null) 
				hor.Visibility = isVert ? Visibility.Collapsed : Visibility.Visible;

			if (vert != null) 
				vert.Visibility = isVert ? Visibility.Visible : Visibility.Collapsed;
		}

		private void UpdateSampleBasedOnManipulation(double x, double y)
        {
            var controlMax = GetControlMax();

            var offsetValue = (IsVertical()) ? controlMax - y : x;
            var controlDist = ControlHelper.CheckBound(offsetValue, controlMax);

            var calculateValue = Minimum;

            if(controlMax != 0)
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
			var offset = ((Value - Minimum)/(Maximum - Minimum))*controlMax;

			var isVert = IsVertical();

			//SetSizeBasedOnOrientation(ProgressRectangle, isVert, offset);

			var thumbItem = GetTemplateChild(isVert ? "VerticalCenterElement" : "HorizontalCenterElement") as FrameworkElement;
			

			if (thumbItem != null)
			{
				var translateTran = thumbItem.RenderTransform as TranslateTransform;

				if (translateTran == null)
					return;

				var thumbItemSize = (isVert ? thumbItem.ActualHeight : thumbItem.ActualWidth);
				var marginOffset = ControlHelper.CheckBound(offset - (thumbItemSize/2d), 0, controlMax - thumbItemSize);

				if (isVert)
				{
					translateTran.X = 0;
					translateTran.Y = marginOffset;
				}
				else
				{
					translateTran.X = marginOffset;
					translateTran.Y = 0;
				}
			}
		}

		#region Helper functions

        private void IsEnabledVisualStateUpdate()
        {
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
        }

        private bool IsVertical()
        {
            return (Orientation == Orientation.Vertical);
        }

        #endregion
    }
}
