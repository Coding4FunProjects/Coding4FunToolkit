using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.Controls.Toolkit.Binding
{
    public class DependencyObjectBinding
    {
        #region CancelGestureListenerBubble
        #region DependencyProperty
        public static bool GetCancelGestureListenerBubble(DependencyObject obj)
        {
            return (bool)obj.GetValue(CancelGestureListenerBubbleProperty);
        }

        public static void SetCancelGestureListenerBubble(DependencyObject obj, bool value)
        {
            obj.SetValue(CancelGestureListenerBubbleProperty, value);
        }

        // Using a DependencyProperty as the backing store for CancelGestureListenerBubble.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelGestureListenerBubbleProperty =
            DependencyProperty.RegisterAttached("CancelGestureListenerBubble", typeof(bool), typeof(DependencyObjectBinding), new PropertyMetadata(false, OnCancelGestureListenerBubbleChanged));
        #endregion

        private static void OnCancelGestureListenerBubbleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            HandleCancelGestureListenerBubble(obj, (bool)e.NewValue);
        }
        #endregion
        private static void HandleCancelGestureListenerBubble(object sender, bool value)
        {
            var item = sender as DependencyObject;

            if (item == null)
                return;

            var gesture = GestureService.GetGestureListener(item);

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

        static void gesture_Cancel(object sender, GestureEventArgs e)
        {
            e.Handled = true;
        }

        //private static void ClipToBoundsPropertyChanged(object sender, RoutedEventArgs e)
        //{
        //    var item = sender as FrameworkElement;

        //    if (item == null)
        //        return;

        //    SetClippingBound(item, GetClipToBounds(item));
        //}

        //private static void SetClippingBound(FrameworkElement element, bool setClippingBound)
        //{
        //    if (setClippingBound)
        //    {
        //        element.Clip =
        //            new RectangleGeometry
        //            {
        //                Rect = new Rect(0, 0, element.ActualWidth, element.ActualHeight)
        //            };
        //    }
        //    else
        //    {
        //        element.Clip = null;
        //    }
        //}
        //#endregion
    }
}
