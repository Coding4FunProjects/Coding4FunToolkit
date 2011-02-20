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
    internal class GestureHelper
    {
        public static void WireUpGestureEvents(bool value, DependencyObject control)
        {
            var gesture = GestureService.GetGestureListener(control);

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
    }
}
