using System;
using System.Linq;
using System.Windows;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
#elif WINDOWS_PHONE
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;
#endif
using Coding4Fun.Toolkit.Controls.Common;
using Windows.UI.Input;
using Windows.UI.Core;

// Code is based off of original code from:
// http://blogs.msdn.com/b/luc/archive/2010/11/22/preventing-the-pivot-or-panorama-controls-from-scrolling.aspx

namespace Coding4Fun.Toolkit.Controls.Binding
{
	public class PreventScrollBinding
	{
		// Contains the Panorama/Pivot. This extension cannot handle blocking
		// multiple panning controls simultaneously.
		private static FrameworkElement _internalPanningControl;

        // Using a DependencyProperty as the backing store for IsScrollSuspended.  This enables animation, styling, binding, etc...
		private static readonly DependencyProperty IsScrollSuspendedProperty =
			DependencyProperty.RegisterAttached("IsScrollSuspended", typeof(bool), typeof(PreventScrollBinding), new PropertyMetadata(false));

		// Using a DependencyProperty as the backing store for LastTouchPoint.  This enables animation, styling, binding, etc...
		private static readonly DependencyProperty LastTouchPointProperty =
			DependencyProperty.RegisterAttached("LastTouchPoint", typeof(Point), typeof(PreventScrollBinding), new PropertyMetadata(null));

		public static bool GetIsEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsEnabled);
		}

		public static void SetIsEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsEnabled, value);
		}

		// Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsEnabled =
			DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(PreventScrollBinding), new PropertyMetadata(false, IsEnabledDependencyPropertyChangedCallback));

		private static void IsEnabledDependencyPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs ea)
		{
			// The element that should prevent the Panorama/Pivot from scrolling
			var blockingElement = dobj as FrameworkElement;

			if (blockingElement == null)
				return;

#if WP8
			blockingElement.UseOptimizedManipulationRouting = false;
#endif

			blockingElement.Unloaded += BlockingElementUnloaded;
			blockingElement.ManipulationStarted += SuspendScroll;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
            blockingElement.PointerPressed += SuspendScroll;
            blockingElement.PointerMoved += SuspendScroll;
#elif WINDOWS_PHONE
            blockingElement.MouseLeftButtonDown += SuspendScroll;
#endif
        }

		private static void BlockingElementUnloaded(object sender, RoutedEventArgs e)
		{
			var blockingElement = sender as FrameworkElement;

			if (blockingElement == null)
				return;

			blockingElement.Unloaded -= BlockingElementUnloaded;
			blockingElement.ManipulationStarted -= SuspendScroll;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
            blockingElement.PointerPressed -= SuspendScroll;
#elif WINDOWS_PHONE
            blockingElement.MouseLeftButtonDown -= SuspendScroll;
#endif
		}

		private static void SuspendScroll(object sender, RoutedEventArgs e)
		{
            
			var blockingElement = sender as FrameworkElement;

			// Determines the parent Panorama/Pivot control
			if (_internalPanningControl == null)
#if WINDOWS_STORE
                _internalPanningControl = FindAncestor(blockingElement, p => p is Hub || p is FlipView) as FrameworkElement;
#elif WINDOWS_PHONE_APP
                _internalPanningControl = FindAncestor(blockingElement, p => p is Pivot || p is Hub || p is FlipView) as FrameworkElement;
#elif WINDOWS_PHONE
                _internalPanningControl = FindAncestor(blockingElement, p => p is Pivot || p is Panorama) as FrameworkElement;
#endif

			if (_internalPanningControl != null && (bool)_internalPanningControl.GetValue(IsScrollSuspendedProperty))
				return;
			// When the user touches the control...
			var originalSource = e.OriginalSource as DependencyObject;
			if (FindAncestor(originalSource, dobj => (dobj == blockingElement)) != blockingElement)
				return;

			// Mark the parent Panorama/Pivot for scroll suspension
			// and register for touch frame events
			if (_internalPanningControl != null)
				_internalPanningControl.SetValue(IsScrollSuspendedProperty, true);

#if WINDOWS_STORE || WINDOWS_PHONE_APP
            CoreWindow.GetForCurrentThread().PointerReleased += PreventScrollBinding_PointerReleased;
#elif WINDOWS_PHONE
            Touch.FrameReported += TouchFrameReported;
#endif
			if (blockingElement != null)
				blockingElement.IsHitTestVisible = true;

			if (_internalPanningControl != null)
				_internalPanningControl.IsHitTestVisible = false;
		}

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private static void PreventScrollBinding_PointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            if (_internalPanningControl == null)
                return;

            // (When the parent Panorama/Pivot is suspended)
            // Wait for the first touch to end (touchaction up). When it is, restore standard
            // panning behavior, otherwise let the control behave normally (no code for this)
            var lastTouchPoint = (Point)_internalPanningControl.GetValue(LastTouchPointProperty);
            var isScrollSuspended = (bool)_internalPanningControl.GetValue(IsScrollSuspendedProperty);

            var touchPoint = args.CurrentPoint.Position;

            if (lastTouchPoint == null || lastTouchPoint != touchPoint)
                lastTouchPoint = touchPoint;

            if (isScrollSuspended)
            {
                // Touch is up, custom behavior is over reset to original values
                if (lastTouchPoint != null)
                {
                    CoreWindow.GetForCurrentThread().PointerReleased -= PreventScrollBinding_PointerReleased;
                    _internalPanningControl.IsHitTestVisible = true;
                    _internalPanningControl.SetValue(IsScrollSuspendedProperty, false);
                }
            }
        }
#elif WINDOWS_PHONE
		private static void TouchFrameReported(object sender, TouchFrameEventArgs e)
		{
			if (_internalPanningControl == null)
				return;

			// (When the parent Panorama/Pivot is suspended)
			// Wait for the first touch to end (touchaction up). When it is, restore standard
			// panning behavior, otherwise let the control behave normally (no code for this)
			var lastTouchPoint = _internalPanningControl.GetValue(LastTouchPointProperty) as TouchPoint;
			var isScrollSuspended = (bool) _internalPanningControl.GetValue(IsScrollSuspendedProperty);

			var touchPoint = e.GetTouchPoints(ApplicationSpace.RootFrame);

			if (lastTouchPoint == null || lastTouchPoint != touchPoint.Last())
				lastTouchPoint = touchPoint.Last();

			if (isScrollSuspended)
			{
				// Touch is up, custom behavior is over reset to original values
				if (lastTouchPoint != null && lastTouchPoint.Action == TouchAction.Up)
				{
					Touch.FrameReported -= TouchFrameReported;
					_internalPanningControl.IsHitTestVisible = true;
					_internalPanningControl.SetValue(IsScrollSuspendedProperty, false);
				}
			}
		}
#endif

		/// <summary>
		/// Traverses the Visual Tree upwards looking for the ancestor that satisfies the <paramref name="predicate"/>.
		/// </summary>
		/// <param name="dependencyObject">The element for which the ancestor is being looked for.</param>
		/// <param name="predicate">The predicate that evaluates if an element is the ancestor that is being looked for.</param>
		/// <returns>
		/// The ancestor element that matches the <paramref name="predicate"/> or <see langword="null"/>
		/// if the ancestor was not found.
		/// </returns>
		public static DependencyObject FindAncestor(DependencyObject dependencyObject, Func<DependencyObject, bool> predicate)
		{
			if (predicate(dependencyObject))
			{
				return dependencyObject;
			}

			DependencyObject parent = null;
			var frameworkElement = dependencyObject as FrameworkElement;

			if (frameworkElement != null)
			{
				parent = frameworkElement.Parent ?? VisualTreeHelper.GetParent(frameworkElement);
			}

			return parent != null ? FindAncestor(parent, predicate) : null;
		}
	}
}
