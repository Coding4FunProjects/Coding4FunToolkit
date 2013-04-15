using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
		private FrameworkElement _horizontalTrack;
		private FrameworkElement _verticalTrack;

        private const string BodyName = "Body";
		private const string HorizontalTemplateName = "HorizontalTemplate";
		private const string VerticalTemplateName = "VerticalTemplate";
		private const string HorizontalTrackName = "HorizontalTrack";
		private const string VerticalTrackName = "VerticalTrack";

		public SuperSliderUpdate()
		{
			DefaultStyleKey = typeof(SuperSliderUpdate);

			IsEnabledChanged += SuperSlider_IsEnabledChanged;
            Loaded += SuperSlider_Loaded;
			SizeChanged += SuperSliderUpdate_SizeChanged;
		}

		void SuperSliderUpdate_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			UpdateValueAndUserInterface(Value, Value);
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

			_horizontalTrack = GetTemplateChild(HorizontalTrackName) as FrameworkElement;
			_verticalTrack = GetTemplateChild(VerticalTrackName) as FrameworkElement;

            var body = GetTemplateChild(BodyName) as Grid;

            if (body != null)
            {
                _monitor = new MovementMonitor();
                _monitor.Movement += _monitor_Movement;
                _monitor.MonitorControl(body);
            }

			AdjustAndUpdateLayout();
			UpdateThumb();
			
//			Dispatcher.BeginInvoke(() =>
				{
					UpdateValueAndUserInterface(Value, Value);
				}
	//		);
        }

        #region dependency properties

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
			DependencyProperty.Register("Thumb", typeof(object), typeof(SuperSliderUpdate), new PropertyMetadata(OnThumbChanged));

		public double TrackSize
        {
			get { return (double)GetValue(TrackSizeProperty); }
			set { SetValue(TrackSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TrackSizeProperty =
			DependencyProperty.Register("TrackSize", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(12d, OnLayoutChanged));

        public double FillSize
        {
			get { return (double)GetValue(FillSizeProperty); }
			set { SetValue(FillSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty FillSizeProperty =
			DependencyProperty.Register("FillSize", typeof(double), typeof(SuperSliderUpdate), new PropertyMetadata(12d, OnLayoutChanged));

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
			DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SuperSliderUpdate), new PropertyMetadata(Orientation.Horizontal, OnOrientationChanged));
        #endregion

        void _monitor_Movement(object sender, MovementMonitorEventArgs e)
        {
            UpdateSampleBasedOnManipulation(e.X, e.Y);
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
			var sender = o as SuperSliderUpdate;

            if (sender != null && e.NewValue != e.OldValue)
				sender.UpdateValueAndUserInterface((double)e.NewValue, (double)e.OldValue);
        }

		private static void OnOrientationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			OnThumbChanged(o, e);
			OnLayoutChanged(o, e);
		}

		private static void OnThumbChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as SuperSliderUpdate;

			if (sender != null && e.NewValue != e.OldValue)
				sender.UpdateThumb();

			OnLayoutChanged(o, e);
		}

        private static void OnLayoutChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
			var sender = o as SuperSliderUpdate;

            if (sender != null && e.NewValue != e.OldValue)
                sender.AdjustAndUpdateLayout();
        }

		private void UpdateThumb()
		{
			var thumbItem = GetTemplateChild(IsVertical() ? "VerticalCenterElement" : "HorizontalCenterElement") as ContentPresenter;

			if (Thumb != null && thumbItem != null) 
				thumbItem.Content = Thumb;

			UpdateUserInterface();
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

            UpdateValue(calculateValue, Value);
        }

        private double GetControlMax()
        {
			return (IsVertical()) ? _verticalTrack.ActualHeight : _horizontalTrack.ActualWidth;
        }

        private void UpdateValue(double newValue, double oldValue)
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

			//if (oldValue == newValue)
			//	return;

            Value = newValue;

			if (ValueChanged != null)
				ValueChanged(this, new RoutedPropertyChangedEventArgs<double>(oldValue, Value));
		}

		private void UpdateValueAndUserInterface(double newValue, double oldValue)
		{
			UpdateValue(newValue, oldValue);
			UpdateUserInterface();
		}

		private void UpdateUserInterface()
		{
			if (!_isLayoutInit)
				return;

			var isVert = IsVertical();

			var thumbItem = GetTemplateChild(isVert ? "VerticalCenterElement" : "HorizontalCenterElement") as FrameworkElement;
			
			if (thumbItem != null)
			{
				var translateTran = thumbItem.RenderTransform as TranslateTransform;

				if (translateTran == null)
					return;

				var thumbItemSize = (isVert ? thumbItem.ActualHeight : thumbItem.ActualWidth);

				var controlMax = GetControlMax() - thumbItemSize;
				var offset = ((Value - Minimum) / (Maximum - Minimum)) * controlMax;
				
				if (isVert)
				{
					translateTran.X = 0;
					translateTran.Y = -offset;

					var fillTarget = GetTemplateChild("VerticalFill") as FrameworkElement;

					if (fillTarget != null)
						fillTarget.Height = offset;
				}
				else
				{
					translateTran.X = offset;
					translateTran.Y = 0;

					var fillTarget = GetTemplateChild("HorizontalFill") as FrameworkElement;

					if (fillTarget != null)
						fillTarget.Width = offset;
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
