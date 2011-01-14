using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Clarity.Phone.Extensions;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.Controls
{
    public class PopUpEventArgs<T> : EventArgs
    {
        public PopUpResult PopUpResult { get; set; }
        public Exception Error { get; set; }
        public T Result { get; set; }
    }

    public enum PopUpResult
    {
        OK,
        Cancelled
    }

    public abstract class PopUp<T> : Control
    {
        private DialogService _popUp;

        private bool _alreadyFired;
        private bool _hasHookedUpGestureWatcher;

        public event EventHandler<PopUpEventArgs<T>> Completed;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if(!_hasHookedUpGestureWatcher)
                WireUpGestureEvents(HasGesturesDisabled);
        }

        protected virtual void OnCompleted(PopUpEventArgs<T> result)
        {
            _alreadyFired = true;
            
            if (Completed != null)
                Completed.Invoke(this, result);
            
            _popUp.Hide();
        }

        public bool IsOpen { get { return _popUp != null && _popUp.IsOpen; } }

        public virtual void Show()
        {
            _popUp = new DialogService
                         {
                             Child = this,
                             BackgroundBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
                         };

            _popUp.Show();

            _popUp.Closed += _popUp_Closed;
        }

        void _popUp_Closed(object sender, EventArgs e)
        {
            if(!_alreadyFired)
                OnCompleted(new PopUpEventArgs<T> { PopUpResult = PopUpResult.Cancelled });

            _popUp = null;
        }

        public bool HasGesturesDisabled
        {
            get { return (bool)GetValue(HasGesturesDisabledProperty); }
            set { SetValue(HasGesturesDisabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasGesturesDisabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasGesturesDisabledProperty =
            DependencyProperty.Register("HasGesturesDisabled", typeof(bool), typeof(PopUp<T>), new PropertyMetadata(true, OnHasGesturesDisabledChanged));

        private static void OnHasGesturesDisabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = ((PopUp<T>)o);
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
    }
}