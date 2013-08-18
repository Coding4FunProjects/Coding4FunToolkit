// Licensed under the Microsoft Public License (Ms-PL) (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://opensource.org/licenses/Ms-PL.html
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// This is a work derived from Microsoft Window Phone Toolkit 
// also released under Ms-PL. See: http://phone.codeplex.com/
// 
// This is a work derived from Tim Heuer's Callisto 
// also released under Ms-PL. See: https://github.com/timheuer/Callisto

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#if WINDOWS_STORE

using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

#elif WINDOWS_PHONE

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;

#endif

namespace Coding4Fun.Toolkit.Controls
{
    /// <summary>
    /// This code provides attached properties for adding a 'tilt' effect to all 
    /// controls within a container.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [SuppressMessage("Microsoft.Design", "CA1052:StaticHolderTypesShouldBeSealed", Justification = "Cannot be static and derive from DependencyObject.")]
	public class TiltEffect : DependencyObject
	{
		/// <summary>
		/// Cache of previous cache modes. Not using weak references for now.
		/// </summary>
		private static readonly Dictionary<DependencyObject, CacheMode> OriginalCacheMode = new Dictionary<DependencyObject, CacheMode>();

		/// <summary>
		/// Maximum amount of tilt, in radians.
		/// </summary>
		private const double MaxAngle = 0.3;

		/// <summary>
		/// Maximum amount of depression, in pixels
		/// </summary>
		private const double MaxDepression = 25;

		private static bool _hasStarted;

		/// <summary>
		/// Delay between releasing an element and the tilt release animation 
		/// playing.
		/// </summary>
		private static readonly TimeSpan TiltReturnAnimationDelay = TimeSpan.FromMilliseconds(200);

		/// <summary>
		/// Duration of tilt release animation.
		/// </summary>
		private static readonly TimeSpan TiltReturnAnimationDuration = TimeSpan.FromMilliseconds(100);

		/// <summary>
		/// The control that is currently being tilted.
		/// </summary>
		private static FrameworkElement _currentTiltElement;

		/// <summary>
		/// The single instance of a storyboard used for all tilts.
		/// </summary>
		private static Storyboard _tiltReturnStoryboard;

		/// <summary>
		/// The single instance of an X rotation used for all tilts.
		/// </summary>
		private static DoubleAnimation _tiltReturnXAnimation;

		/// <summary>
		/// The single instance of a Y rotation used for all tilts.
		/// </summary>
		private static DoubleAnimation _tiltReturnYAnimation;

		/// <summary>
		/// The single instance of a Z depression used for all tilts.
		/// </summary>
		private static DoubleAnimation _tiltReturnZAnimation;

		/// <summary>
		/// The center of the tilt element.
		/// </summary>
		private static Point _currentTiltElementCenter;

		/// <summary>
		/// Whether the animation just completed was for a 'pause' or not.
		/// </summary>
		private static bool _wasPauseAnimation;

		/// <summary>
		/// Whether to use a slightly more accurate (but slightly slower) tilt 
		/// animation easing function.
		/// </summary>
		public static bool UseLogarithmicEase { get; set; }

		/// <summary>
		/// Default list of items that are tiltable.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tiltable", Justification = "By design.")]
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Keeping it simple.")]
		public static List<Type> TiltableItems { get; private set; }

		#region Constructor and Static Constructor
		/// <summary>
		/// This is not a constructable class, but it cannot be static because 
		/// it derives from DependencyObject.
		/// </summary>
		private TiltEffect() { }

	    static TiltEffect()
		{
			// The tiltable items list.
			TiltableItems = new List<Type>
				{ 
                typeof(ButtonBase), 
                typeof(ListBoxItem), 
            };
		}

		#endregion

		#region Dependency properties

		/// <summary>
		/// Whether the tilt effect is enabled on a container (and all its 
		/// children).
		/// </summary>
		public static readonly DependencyProperty IsTiltEnabledProperty = DependencyProperty.RegisterAttached(
		  "IsTiltEnabled",
		  typeof(bool),
		  typeof(TiltEffect),
		  new PropertyMetadata(false, OnIsTiltEnabledChanged)
		  );

		/// <summary>
		/// Gets the IsTiltEnabled dependency property from an object.
		/// </summary>
		/// <param name="source">The object to get the property from.</param>
		/// <returns>The property's value.</returns>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Standard pattern.")]
		public static bool GetIsTiltEnabled(DependencyObject source)
		{
			return (bool)source.GetValue(IsTiltEnabledProperty);
		}

		/// <summary>
		/// Sets the IsTiltEnabled dependency property on an object.
		/// </summary>
		/// <param name="source">The object to set the property on.</param>
		/// <param name="value">The value to set.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Standard pattern.")]
		public static void SetIsTiltEnabled(DependencyObject source, bool value)
		{
			source.SetValue(IsTiltEnabledProperty, value);
		}

		/// <summary>
		/// Suppresses the tilt effect on a single control that would otherwise 
		/// be tilted.
		/// </summary>
		public static readonly DependencyProperty SuppressTiltProperty = DependencyProperty.RegisterAttached(
		  "SuppressTilt",
		  typeof(bool),
		  typeof(TiltEffect),
		  null
		  );

		/// <summary>
		/// Gets the SuppressTilt dependency property from an object.
		/// </summary>
		/// <param name="source">The object to get the property from.</param>
		/// <returns>The property's value.</returns>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Standard pattern.")]
		public static bool GetSuppressTilt(DependencyObject source)
		{
			return (bool)source.GetValue(SuppressTiltProperty);
		}

		/// <summary>
		/// Sets the SuppressTilt dependency property from an object.
		/// </summary>
		/// <param name="source">The object to get the property from.</param>
		/// <param name="value">The property's value.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Standard pattern.")]
		public static void SetSuppressTilt(DependencyObject source, bool value)
		{
			source.SetValue(SuppressTiltProperty, value);
		}

		/// <summary>
		/// Property change handler for the IsTiltEnabled dependency property.
		/// </summary>
		/// <param name="target">The element that the property is atteched to.</param>
		/// <param name="args">Event arguments.</param>
		/// <remarks>
		/// Adds or removes event handlers from the element that has been 
		/// (un)registered for tilting.
		/// </remarks>
		private static void OnIsTiltEnabledChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
		{
			var element = target as FrameworkElement;

			if (element == null)
				return;

			if (element.GetVisualParent() == null)
			{
				element.Loaded += element_Loaded;
				return;
			}

			// Add / remove the event handler if necessary
			StateManagementForIsTiltEnabled((bool)args.NewValue, element);
		}

		static void element_Loaded(object sender, RoutedEventArgs e)
		{
			var element = sender as FrameworkElement;
			StateManagementForIsTiltEnabled(GetIsTiltEnabled(sender as DependencyObject), element);

			if (element != null)
				element.Loaded -= element_Loaded;
		}

		private static void StateManagementForIsTiltEnabled(bool isEnabled, FrameworkElement element)
		{
			if (isEnabled)
			{
				AddToValidControls(element);
			}
			else
			{
				RemoveFromValidControls(element);
			}
		}


		#endregion

	    private static void AddToValidControls(UIElement element)
	    {
			bool isTiltable = false;
			bool isForcedSuppress = element.ReadLocalValue(SuppressTiltProperty) is bool && (bool) element.ReadLocalValue(SuppressTiltProperty);


			if (!isForcedSuppress && TiltableItems.Any(t => TypeExtensions.IsTypeOf(element, t)))
		    {
			    isTiltable = true;

#if WINDOWS_STORE
				element.PointerMoved += InitiateTiltEffect;
#elif WINDOWS_PHONE
				element.ManipulationStarted += InitiateTiltEffect;
#endif
		    }

		    // hit a child with a tilt ability, 
			// having a control within a control having tilt is redundant
		    if (isTiltable)
				return;

		    var children = element.GetVisualChildren();

		    foreach (var child in children)
		    {
			    AddToValidControls(child as FrameworkElement);
		    }
	    }

	    private static void RemoveFromValidControls(FrameworkElement element)
	    {
		    var children = element.GetVisualChildren().OfType<FrameworkElement>();

		    foreach (var child in children)
			{
#if WINDOWS_STORE
				child.PointerMoved -= InitiateTiltEffect;
#elif WINDOWS_PHONE
				child.ManipulationStarted -= InitiateTiltEffect;
#endif
		    }
	    }

#if WINDOWS_STORE
		private static void InitiateTiltEffect(object sender, PointerRoutedEventArgs e)
#elif WINDOWS_PHONE
		private static void InitiateTiltEffect(object sender, ManipulationStartedEventArgs e)
#endif
		{
			var element = sender as FrameworkElement;

			if (_hasStarted) 
				return;

#if WINDOWS_STORE

			if (
				(e.Pointer.PointerDeviceType == PointerDeviceType.Touch) ||
				(e.Pointer.PointerDeviceType != PointerDeviceType.Touch && e.Pointer.IsInContact) // mouse and pen
				)
			{
				if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch && element != null)
				{
					element.PointerExited += TiltEffectExited;
				}
			}
#endif

			TryStartTiltEffect(element, e);
			_hasStarted = true;
		}
#if WINDOWS_STORE

		/// <summary>
		/// Event handler for ManipulationStarted.
		/// </summary>
		/// <param name="sender">sender of the event - this will be the tilt 
		/// container (eg, entire page).</param>
		/// <param name="e">Event arguments.</param>
		private static void TiltEffect_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			TryStartTiltEffect(sender as FrameworkElement, e);
		}

		/// <summary>
		/// Event handler for ManipulationDelta
		/// </summary>
		/// <param name="sender">sender of the event - this will be the tilting 
		/// object (eg a button).</param>
		/// <param name="e">Event arguments.</param>
		private static void TiltEffect_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			ContinueTiltEffect(sender as FrameworkElement, e);
		}

		/// <summary>
		/// Event handler for ManipulationCompleted.
		/// </summary>
		/// <param name="sender">sender of the event - this will be the tilting 
		/// object (eg a button).</param>
		/// <param name="e">Event arguments.</param>
		private static void TiltEffect_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			EndTiltEffect();
		}

		/// <summary>
		/// Event handler for PointerCaptureLost
		/// </summary>
		/// <param name="sender">sender of the event - this will be the tilting 
		/// object (eg a button).</param>
		/// <param name="e">Event arguments.</param>
		private static void TiltEffect_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
		{
			EndTiltEffect();
		}

		static void TiltEffectExited(object sender, PointerRoutedEventArgs e)
		{
			EndTiltEffect();
		}


#elif WINDOWS_PHONE
		/// <summary>
        /// Event handler for ManipulationDelta
        /// </summary>
        /// <param name="sender">sender of the event - this will be the tilting 
        /// object (eg a button).</param>
        /// <param name="e">Event arguments.</param>
        private static void TiltEffectDelta(object sender, ManipulationDeltaEventArgs e)
        {
            ContinueTiltEffect(sender as FrameworkElement, e);
        }

        /// <summary>
        /// Event handler for ManipulationCompleted.
        /// </summary>
        /// <param name="sender">sender of the event - this will be the tilting 
        /// object (eg a button).</param>
        /// <param name="e">Event arguments.</param>
        static void TiltEffectCompleted(object sender, ManipulationCompletedEventArgs e)
        {
	        EndTiltEffect();
        }
#endif
	    #region Top-level manipulation event handlers

		#endregion

		#region Core tilt logic

		private static void EndTiltEffect()
		{
			_hasStarted = false;
#if WINDOWS_STORE
			_currentTiltElement.PointerExited -= TiltEffectExited;
#endif
			EndTiltEffect(_currentTiltElement);
		}

		
#if WINDOWS_STORE
		private static void TryStartTiltEffect(FrameworkElement source, PointerRoutedEventArgs e)
#elif WINDOWS_PHONE
		private static void TryStartTiltEffect(FrameworkElement source, ManipulationStartedEventArgs e)
#endif
	    {
			// Use first child of the control, so that we can add transforms and not
			// impact any transforms on the control itself.


#if WINDOWS_STORE
			var element = source;
			var container = source;
#elif WINDOWS_PHONE
			var element = VisualTreeHelper.GetChild(source, 0) as FrameworkElement;
		    var container = e.ManipulationContainer as FrameworkElement;
#endif
			if (element == null || container == null)
			    return;

		    // Touch point relative to the element being tilted.
#if WINDOWS_STORE
			var tiltTouchPoint = e.GetCurrentPoint(element).Position;
#elif WINDOWS_PHONE
		    var tiltTouchPoint = container.TransformToVisual(element).Transform(e.ManipulationOrigin);
#endif
			// Center of the element being tilted.
		    var elementCenter = new Point(element.ActualWidth/2, element.ActualHeight/2);

		    // Camera adjustment.
		    var centerToCenterDelta = GetCenterToCenterDelta(element, source);

#if WINDOWS_STORE
			BeginTiltEffect(element, tiltTouchPoint, elementCenter, centerToCenterDelta, e.Pointer);
#elif WINDOWS_PHONE
			BeginTiltEffect(element, tiltTouchPoint, elementCenter, centerToCenterDelta);
#endif
		}

		/// <summary>
		/// Computes the delta between the centre of an element and its 
		/// container.
		/// </summary>
		/// <param name="element">The element to compare.</param>
		/// <param name="container">The element to compare against.</param>
		/// <returns>A point that represents the delta between the two centers.</returns>
		static Point GetCenterToCenterDelta(FrameworkElement element, FrameworkElement container)
		{
			var elementCenter = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
			Point containerCenter;

#if WINDOWS_PHONE

			// Need to special-case the frame to handle different orientations.
			var frame = container as PhoneApplicationFrame;
			if (frame != null)
			{
				// Switch width and height in landscape mode
				containerCenter = 
					((frame.Orientation & PageOrientation.Landscape) == PageOrientation.Landscape) ? 
					new Point(container.ActualHeight / 2, container.ActualWidth / 2) : 
					new Point(container.ActualWidth / 2, container.ActualHeight / 2);
			}
			else
			{
				containerCenter = new Point(container.ActualWidth / 2, container.ActualHeight / 2);
			}
#else
            containerCenter = new Point(container.ActualWidth / 2, container.ActualHeight / 2);
#endif

#if WINDOWS_STORE	
			var transformedElementCenter = element.TransformToVisual(container).TransformPoint(elementCenter);
#elif WINDOWS_PHONE
			var transformedElementCenter = element.TransformToVisual(container).Transform(elementCenter);
#endif		
			
			return new Point(containerCenter.X - transformedElementCenter.X, containerCenter.Y - transformedElementCenter.Y);
		}

		/// <summary>
		/// Begins the tilt effect by preparing the control and doing the 
		/// initial animation.
		/// </summary>
		/// <param name="element">The element to tilt.</param>
		/// <param name="touchPoint">The touch point, in element coordinates.</param>
		/// <param name="centerPoint">The center point of the element in element
		/// coordinates.</param>
		/// <param name="centerDelta">The delta between the 
		/// <paramref name="element"/>'s center and the container's center.</param>
#if WINDOWS_STORE
			static void BeginTiltEffect(FrameworkElement element, Point touchPoint, Point centerPoint, Point centerDelta, Pointer p)
#elif WINDOWS_PHONE
			static void BeginTiltEffect(FrameworkElement element, Point touchPoint, Point centerPoint, Point centerDelta)
#endif
			
		{
			if (_tiltReturnStoryboard != null)
			{
				StopTiltReturnStoryboardAndCleanup();
			}

#if WINDOWS_STORE
			if (!PrepareControlForTilt(element, centerDelta, p))
#elif WINDOWS_PHONE
			if (!PrepareControlForTilt(element, centerDelta))
#endif
			{
				return;
			}

			_currentTiltElement = element;
			_currentTiltElementCenter = centerPoint;
			PrepareTiltReturnStoryboard(element);

			ApplyTiltEffect(_currentTiltElement, touchPoint, _currentTiltElementCenter);
		}

		/// <summary>
		/// Prepares a control to be tilted by setting up a plane projection and
		/// some event handlers.
		/// </summary>
		/// <param name="element">The control that is to be tilted.</param>
		/// <param name="centerDelta">Delta between the element's center and the
		/// tilt container's.</param>
		/// <returns>true if successful; false otherwise.</returns>
		/// <remarks>
		/// This method is conservative; it will fail any attempt to tilt a 
		/// control that already has a projection on it.
		/// </remarks>
#if WINDOWS_STORE
			static bool PrepareControlForTilt(FrameworkElement element, Point centerDelta, Pointer p)
#elif WINDOWS_PHONE
			static bool PrepareControlForTilt(FrameworkElement element, Point centerDelta)
#endif
		{
			// Prevents interference with any existing transforms
			if (element.Projection != null || (element.RenderTransform != null && element.RenderTransform.GetType() != typeof(MatrixTransform)))
			{
				return false;
			}

			OriginalCacheMode[element] = element.CacheMode;
			element.CacheMode = new BitmapCache();

			var transform = new TranslateTransform {X = centerDelta.X, Y = centerDelta.Y};
			element.RenderTransform = transform;

			var projection = new PlaneProjection {GlobalOffsetX = -1*centerDelta.X, GlobalOffsetY = -1*centerDelta.Y};
			element.Projection = projection;

#if WINDOWS_STORE
			element.PointerMoved += TiltEffect_PointerMoved;
			element.PointerReleased += TiltEffect_PointerReleased;
			element.PointerCaptureLost += TiltEffect_PointerCaptureLost;
			element.CapturePointer(p);
#elif WINDOWS_PHONE
			element.ManipulationDelta += TiltEffectDelta;
			element.ManipulationCompleted += TiltEffectCompleted;
#endif
			return true;
		}

		/// <summary>
		/// Removes modifications made by PrepareControlForTilt.
		/// </summary>
		/// <param name="element">THe control to be un-prepared.</param>
		/// <remarks>
		/// This method is basic; it does not do anything to detect if the 
		/// control being un-prepared was previously prepared.
		/// </remarks>
		private static void RevertPrepareControlForTilt(FrameworkElement element)
		{
#if WINDOWS_STORE
			element.PointerMoved -= TiltEffect_PointerMoved;
			element.PointerReleased -= TiltEffect_PointerReleased;
			element.PointerCaptureLost -= TiltEffect_PointerCaptureLost;
#elif WINDOWS_PHONE
			element.ManipulationDelta -= TiltEffectDelta;
			element.ManipulationCompleted -= TiltEffectCompleted;
#endif

			element.Projection = null;
			element.RenderTransform = null;
			CacheMode original;
			
			if (OriginalCacheMode.TryGetValue(element, out original))
			{
				element.CacheMode = original;
				OriginalCacheMode.Remove(element);
			}
			else
			{
				element.CacheMode = null;
			}
		}

		/// <summary>
		/// Creates the tilt return storyboard (if not already created) and 
		/// targets it to the projection.
		/// </summary>
		/// <param name="element">The framework element to prepare for
		/// projection.</param>
		static void PrepareTiltReturnStoryboard(FrameworkElement element)
		{
			if (_tiltReturnStoryboard == null)
			{
				_tiltReturnStoryboard = new Storyboard();
				_tiltReturnStoryboard.Completed += TiltReturnStoryboard_Completed;


#if WINDOWS_STORE
				_tiltReturnXAnimation = MakeDoubleAnimation("RotationX");
				_tiltReturnYAnimation = MakeDoubleAnimation("RotationY");
				_tiltReturnZAnimation = MakeDoubleAnimation("GlobalOffsetZ");
#elif WINDOWS_PHONE
				_tiltReturnXAnimation = MakeDoubleAnimation(PlaneProjection.RotationXProperty);
				_tiltReturnYAnimation = MakeDoubleAnimation(PlaneProjection.RotationYProperty);
				_tiltReturnZAnimation = MakeDoubleAnimation(PlaneProjection.GlobalOffsetZProperty);
#endif

				_tiltReturnStoryboard.Children.Add(_tiltReturnXAnimation);
				_tiltReturnStoryboard.Children.Add(_tiltReturnYAnimation);
				_tiltReturnStoryboard.Children.Add(_tiltReturnZAnimation);
			}

			Storyboard.SetTarget(_tiltReturnXAnimation, element.Projection);
			Storyboard.SetTarget(_tiltReturnYAnimation, element.Projection);
			Storyboard.SetTarget(_tiltReturnZAnimation, element.Projection);
		}

#if WINDOWS_STORE
		private static DoubleAnimation MakeDoubleAnimation(string property)
#elif WINDOWS_PHONE
		private static DoubleAnimation MakeDoubleAnimation(DependencyProperty property)
#endif
	    {
			var animation = new DoubleAnimation();
#if WINDOWS_STORE
			Storyboard.SetTargetProperty(animation, property);
#elif WINDOWS_PHONE
			Storyboard.SetTargetProperty(animation, new PropertyPath(property));
#endif
			animation.BeginTime = TiltReturnAnimationDelay;
			animation.To = 0;
			animation.Duration = TiltReturnAnimationDuration;

			return animation;
	    }

	    /// <summary>
		/// Continues a tilt effect that is currently applied to an element, 
		/// presumably because the user moved their finger.
		/// </summary>
		/// <param name="element">The element being tilted.</param>
		/// <param name="e">The manipulation event args.</param>
#if WINDOWS_STORE
		static void ContinueTiltEffect(FrameworkElement element, PointerRoutedEventArgs e)
#elif WINDOWS_PHONE
		static void ContinueTiltEffect(FrameworkElement element, ManipulationDeltaEventArgs e)
#endif
		{
#if WINDOWS_STORE
			var container = element;
#elif WINDOWS_PHONE
			var container = e.ManipulationContainer as FrameworkElement;
#endif

			if (container == null || element == null)
			{
				return;
			}

#if WINDOWS_STORE
			var tiltTouchPoint = e.GetCurrentPoint(element).Position;
#elif WINDOWS_PHONE
			var tiltTouchPoint = container.TransformToVisual(element).Transform(e.ManipulationOrigin);
#endif

			// If touch moved outside bounds of element, then pause the tilt 
			// (but don't cancel it)
			if (new Rect(0, 0, _currentTiltElement.ActualWidth, _currentTiltElement.ActualHeight).Contains(tiltTouchPoint) != true)
			{
				PauseTiltEffect();
			}
			else
			{
				// Apply the updated tilt effect
				ApplyTiltEffect(_currentTiltElement, tiltTouchPoint, _currentTiltElementCenter);
			}
		}

		/// <summary>
		/// Ends the tilt effect by playing the animation.
		/// </summary>
		/// <param name="element">The element being tilted.</param>
		private static void EndTiltEffect(FrameworkElement element)
		{
			if (element != null)
			{
#if WINDOWS_STORE
				element.PointerReleased -= TiltEffect_PointerPressed;
				element.PointerMoved -= TiltEffect_PointerMoved;
#elif WINDOWS_PHONE
				element.ManipulationCompleted -= TiltEffectCompleted;
				element.ManipulationDelta -= TiltEffectDelta;
#endif
			}

			if (_tiltReturnStoryboard != null)
			{
				_wasPauseAnimation = false;
				if (_tiltReturnStoryboard.GetCurrentState() != ClockState.Active)
				{
					_tiltReturnStoryboard.Begin();
				}
			}
			else
			{
				StopTiltReturnStoryboardAndCleanup();
			}
		}

		/// <summary>
		/// Handler for the storyboard complete event.
		/// </summary>
		/// <param name="sender">sender of the event.</param>
		/// <param name="e">event args.</param>
#if WINDOWS_STORE
		private static void TiltReturnStoryboard_Completed(object sender, object e)
#elif WINDOWS_PHONE
		private static void TiltReturnStoryboard_Completed(object sender, EventArgs e)
#endif
		{
			if (_wasPauseAnimation)
			{
				ResetTiltEffect(_currentTiltElement);
			}
			else
			{
				StopTiltReturnStoryboardAndCleanup();
			}
		}

		/// <summary>
		/// Resets the tilt effect on the control, making it appear 'normal'
		/// again.
		/// </summary>
		/// <param name="element">The element to reset the tilt on.</param>
		/// <remarks>
		/// This method doesn't turn off the tilt effect or cancel any current
		/// manipulation; it just temporarily cancels the effect.
		/// </remarks>
		private static void ResetTiltEffect(FrameworkElement element)
		{
			var projection = element.Projection as PlaneProjection;
			
			if (projection != null)
			{
				projection.RotationY = 0;
				projection.RotationX = 0;
				projection.GlobalOffsetZ = 0;
			}
		}

		/// <summary>
		/// Stops the tilt effect and release resources applied to the currently
		/// tilted control.
		/// </summary>
		private static void StopTiltReturnStoryboardAndCleanup()
		{
			if (_tiltReturnStoryboard != null)
			{
				_tiltReturnStoryboard.Stop();
			}

			if (_currentTiltElement != null)
			{
				RevertPrepareControlForTilt(_currentTiltElement);
				_currentTiltElement = null;
			}
		}

		/// <summary>
		/// Pauses the tilt effect so that the control returns to the 'at rest'
		/// position, but doesn't stop the tilt effect (handlers are still 
		/// attached).
		/// </summary>
		private static void PauseTiltEffect()
		{
			if ((_tiltReturnStoryboard != null) && !_wasPauseAnimation)
			{
				_tiltReturnStoryboard.Stop();
				_wasPauseAnimation = true;
				_tiltReturnStoryboard.Begin();
			}
		}

		/// <summary>
		/// Resets the storyboard to not running.
		/// </summary>
		private static void ResetTiltReturnStoryboard()
		{
			_tiltReturnStoryboard.Stop();
			_wasPauseAnimation = false;
		}

		/// <summary>
		/// Applies the tilt effect to the control.
		/// </summary>
		/// <param name="element">the control to tilt.</param>
		/// <param name="touchPoint">The touch point, in the container's 
		/// coordinates.</param>
		/// <param name="centerPoint">The center point of the container.</param>
		private static void ApplyTiltEffect(FrameworkElement element, Point touchPoint, Point centerPoint)
		{
			// Stop any active animation
			ResetTiltReturnStoryboard();

			// Get relative point of the touch in percentage of container size
			var normalizedPoint = new Point(
				Math.Min(Math.Max(touchPoint.X / (centerPoint.X * 2), 0), 1),
				Math.Min(Math.Max(touchPoint.Y / (centerPoint.Y * 2), 0), 1));

			if (double.IsNaN(normalizedPoint.X) || double.IsNaN(normalizedPoint.Y))
			{
				return;
			}

			// Shell values
			double xMagnitude = Math.Abs(normalizedPoint.X - 0.5);
			double yMagnitude = Math.Abs(normalizedPoint.Y - 0.5);
			double xDirection = -Math.Sign(normalizedPoint.X - 0.5);
			double yDirection = Math.Sign(normalizedPoint.Y - 0.5);
			double angleMagnitude = xMagnitude + yMagnitude;
			double xAngleContribution = xMagnitude + yMagnitude > 0 ? xMagnitude / (xMagnitude + yMagnitude) : 0;

			double angle = angleMagnitude * MaxAngle * 180 / Math.PI;
			double depression = (1 - angleMagnitude) * MaxDepression;

			// RotationX and RotationY are the angles of rotations about the x- 
			// or y-*axis*; to achieve a rotation in the x- or y-*direction*, we 
			// need to swap the two. That is, a rotation to the left about the 
			// y-axis is a rotation to the left in the x-direction, and a 
			// rotation up about the x-axis is a rotation up in the y-direction.
			var projection = element.Projection as PlaneProjection;
			
			if (projection != null)
			{
				projection.RotationY = angle * xAngleContribution * xDirection;
				projection.RotationX = angle * (1 - xAngleContribution) * yDirection;
				projection.GlobalOffsetZ = -depression;
			}
		}

		#endregion
	}
}
