using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Clarity.Phone.Extensions;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.Controls
{
    public abstract class PopUp<T, TPopUpResult> : Control
    {
        private DialogService _popUp;

        private bool _alreadyFired;
        private bool _hasHookedUpGestureWatcher;

        public event EventHandler<PopUpEventArgs<T, TPopUpResult>> Completed;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if(!_hasHookedUpGestureWatcher)
                WireUpGestureEvents(HasGesturesDisabled);
        }

        protected virtual void OnCompleted(PopUpEventArgs<T, TPopUpResult> result)
        {
            _alreadyFired = true;
            
            if (Completed != null)
                Completed.Invoke(this, result);
            
            if(_popUp != null)
                _popUp.Hide();
        }

        public bool IsOpen { get { return _popUp != null && _popUp.IsOpen; } }

        public virtual void Show()
        {
            _popUp = new DialogService
                         {
                             Child = this,
                             BackgroundBrush = Overlay,
                         };
            
            _popUp.Closed += _popUp_Closed;

            Dispatcher.BeginInvoke(_popUp.Show);
        }

        protected virtual TPopUpResult GetOnClosedValue()
        {
            return default(TPopUpResult);
        }

        void _popUp_Closed(object sender, EventArgs e)
        {
            if(!_alreadyFired)
                OnCompleted(new PopUpEventArgs<T, TPopUpResult> { PopUpResult = GetOnClosedValue() });

            _popUp = null;
        }



        public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Overlay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayProperty =
            DependencyProperty.Register("Overlay", typeof(Brush), typeof(PopUp<T, TPopUpResult>), new PropertyMetadata(Application.Current.Resources["PhoneSemitransparentBrush"]));

        


        public bool HasGesturesDisabled
        {
            get { return (bool)GetValue(HasGesturesDisabledProperty); }
            set { SetValue(HasGesturesDisabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasGesturesDisabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasGesturesDisabledProperty =
            DependencyProperty.Register("HasGesturesDisabled", typeof(bool), typeof(PopUp<T, TPopUpResult>), new PropertyMetadata(true, OnHasGesturesDisabledChanged));

        private static void OnHasGesturesDisabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = ((PopUp<T, TPopUpResult>)o);
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