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
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.Controls
{
    public class ProgressOverlay : Control
    {
        private Storyboard fadeIn;
        private Storyboard fadeOut;
        private Grid LayoutGrid;

        private const string fadeInName = "fadeIn";
        private const string fadeOutName = "fadeOut";
        private const string LayoutGridName = "LayoutGrid";

        private bool _hasHookedUpGestureWatcher = false;

        public ProgressOverlay()
        {
            DefaultStyleKey = typeof(ProgressOverlay);
            DataContext = this;
        }

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(ProgressOverlay), new PropertyMetadata(null));

        public bool HasGesturesDisabled
        {
            get { return (bool)GetValue(HasGesturesDisabledProperty); }
            set { SetValue(HasGesturesDisabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasGesturesDisabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasGesturesDisabledProperty =
            DependencyProperty.Register("HasGesturesDisabled", typeof(bool), typeof(ProgressOverlay), new PropertyMetadata(true, OnHasGesturesDisabledChanged));

        private static void OnHasGesturesDisabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = ((ProgressOverlay)o);
            if (sender != null && e.NewValue != e.OldValue)
                sender.WireUpGestureEvents((bool)e.NewValue);
        }

        private void WireUpGestureEvents(bool value)
        {
            _hasHookedUpGestureWatcher = true;

            var gesture = GestureService.GetGestureListener(this);

            if (value)
            {
                gesture.DoubleTap += gesture_Cancel;
                gesture.DragCompleted += gesture_Cancel;
                gesture.DragDelta += gesture_Cancel;
                gesture.DragStarted += gesture_Cancel;
                gesture.Flick += gesture_Cancel;
                gesture.GestureBegin += gesture_Cancel;
                gesture.GestureCompleted += gesture_Cancel;
                gesture.Hold += gesture_Cancel;
                gesture.PinchCompleted += gesture_Cancel;
                gesture.PinchDelta += gesture_Cancel;
                gesture.PinchStarted += gesture_Cancel;
                gesture.Tap += gesture_Cancel;
            }
            else
            {
                gesture.DoubleTap -= gesture_Cancel;
                gesture.DragCompleted -= gesture_Cancel;
                gesture.DragDelta -= gesture_Cancel;
                gesture.DragStarted -= gesture_Cancel;
                gesture.Flick -= gesture_Cancel;
                gesture.GestureBegin -= gesture_Cancel;
                gesture.GestureCompleted -= gesture_Cancel;
                gesture.Hold -= gesture_Cancel;
                gesture.PinchCompleted -= gesture_Cancel;
                gesture.PinchDelta -= gesture_Cancel;
                gesture.PinchStarted -= gesture_Cancel;
                gesture.Tap -= gesture_Cancel;
            }
        }

        void gesture_Cancel(object sender, GestureEventArgs e)
        {
            e.Handled = true;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!_hasHookedUpGestureWatcher)
                WireUpGestureEvents(HasGesturesDisabled);

            fadeIn = GetTemplateChild(fadeInName) as Storyboard;
            fadeOut = GetTemplateChild(fadeOutName) as Storyboard;
            LayoutGrid = GetTemplateChild(LayoutGridName) as Grid;
        }

        public void Show()
        {
            if (fadeIn == null)
                ApplyTemplate();

            if (Visibility == Visibility.Collapsed)
            {
                LayoutGrid.Opacity = 0;
                Visibility = Visibility.Visible;
            }

            if (fadeIn != null) 
                fadeIn.Begin();
        }

        public void Hide()
        {
            if (fadeOut == null)
                ApplyTemplate();

            if (fadeOut != null) 
                fadeOut.Begin();
        }
    }
}
