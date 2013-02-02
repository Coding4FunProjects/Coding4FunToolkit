// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Microsoft.Phone.Controls
{
    /// <summary>
    /// Enables navigation transitions for
    /// <see cref="T:Microsoft.Phone.Controls.PhoneApplicationPage"/>s.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [TemplatePart(Name = FirstTemplatePartName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = SecondTemplatePartName, Type = typeof(ContentPresenter))]
    internal class TransitionFrame : PhoneApplicationFrame
    {
        #region Constants and Statics
        /// <summary>
        /// The new
        /// <see cref="T:System.Windows.Controls.ContentPresenter"/>
        /// template part name.
        /// </summary>
        private const string FirstTemplatePartName = "FirstContentPresenter";

        /// <summary>
        /// The old
        /// <see cref="T:System.Windows.Controls.ContentPresenter"/>
        /// template part name.
        /// </summary>
        private const string SecondTemplatePartName = "SecondContentPresenter";

        /// <summary>
        /// A single shared instance for setting BitmapCache on a visual.
        /// </summary>
        internal static readonly CacheMode BitmapCacheMode = new BitmapCache();
        #endregion

        #region Template Parts
        /// <summary>
        /// The first <see cref="T:System.Windows.Controls.ContentPresenter"/>.
        /// </summary>
        private ContentPresenter _firstContentPresenter;

        /// <summary>
        /// The second <see cref="T:System.Windows.Controls.ContentPresenter"/>.
        /// </summary>
        private ContentPresenter _secondContentPresenter;

        /// <summary>
        /// The new <see cref="T:System.Windows.Controls.ContentPresenter"/>.
        /// </summary>
        private ContentPresenter _newContentPresenter;

        /// <summary>
        /// The old <see cref="T:System.Windows.Controls.ContentPresenter"/>.
        /// </summary>
        private ContentPresenter _oldContentPresenter;
        #endregion

        /// <summary>
        /// Indicates whether a navigation is forward.
        /// </summary>
        private bool _isForwardNavigation;

        /// <summary>
        /// Determines whether to set the new content to the first or second
        /// <see cref="T:System.Windows.Controls.ContentPresenter"/>.
        /// </summary>
        private bool _useFirstAsNew;

        /// <summary>
        /// A value indicating whether the old transition has completed and the
        /// new transition can begin.
        /// </summary>
        private bool _readyToTransitionToNewContent;

        /// <summary>
        /// A value indicating whether the new content has been loaded and the
        /// new transition can begin.
        /// </summary>
        private bool _contentReady;

        /// <summary>
        /// A value indicating whether the exit transition is currently being performed.
        /// </summary>
        private bool _performingExitTransition;

        /// <summary>
        /// The transition to use to move in new content once the old transition
        /// is complete and ready for movement.
        /// </summary>
        private ITransition _storedNewTransition;

        /// <summary>
        /// The stored NavigationIn transition instance to use once the old
        /// transition is complete and ready for movement.
        /// </summary>
        private NavigationInTransition _storedNavigationInTransition;

        /// <summary>
        /// The transition to use to complete the old transition.
        /// </summary>
        private ITransition _storedOldTransition;

        /// <summary>
        /// The stored NavigationOut transition instance.
        /// </summary>
        private NavigationOutTransition _storedNavigationOutTransition;

        /// <summary>
        /// Initialzies a new instance of the TransitionFrame class.
        /// </summary>
        public TransitionFrame()
            : base()
        {
            DefaultStyleKey = typeof(TransitionFrame);
            Navigating += OnNavigating;
            BackKeyPress += OnBackKeyPress;
        }

        /// <summary>
        /// Flips the logical content presenters to prepare for the next visual
        /// transition.
        /// </summary>
        private void FlipPresenters()
        {
            _newContentPresenter = _useFirstAsNew ? _firstContentPresenter : _secondContentPresenter;
            _oldContentPresenter = _useFirstAsNew ? _secondContentPresenter : _firstContentPresenter;
            _useFirstAsNew = !_useFirstAsNew;
        }

        /// <summary>
        /// Handles the Navigating event of the frame, the immediate way to
        /// begin a transition out before the new page has loaded or had its
        /// layout pass.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event arguments.</param>
        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            _isForwardNavigation = e.NavigationMode != NavigationMode.Back;

            var oldElement = Content as UIElement;
            if (oldElement == null)
            {
                return;
            }

            FlipPresenters();

            TransitionElement oldTransitionElement = null;
            NavigationOutTransition navigationOutTransition = null;
            ITransition oldTransition = null;

            navigationOutTransition = TransitionService.GetNavigationOutTransition(oldElement);

            if (navigationOutTransition != null)
            {
                oldTransitionElement = _isForwardNavigation ? navigationOutTransition.Forward : navigationOutTransition.Backward;
            }
            if (oldTransitionElement != null)
            {
                oldTransition = oldTransitionElement.GetTransition(oldElement);
            }
            if (oldTransition != null)
            {
                EnsureStoppedTransition(oldTransition);

                _storedNavigationOutTransition = navigationOutTransition;
                _storedOldTransition = oldTransition;
                oldTransition.Completed += OnExitTransitionCompleted;

                _performingExitTransition = true;

                PerformTransition(navigationOutTransition, _oldContentPresenter, oldTransition);

                PrepareContentPresenterForCompositor(_oldContentPresenter);
            }
            else
            {
                _readyToTransitionToNewContent = true;
            }
        }

        /// <summary>
        /// Handles the completion of the exit transition, automatically 
        /// continuing to bring in the new element's transition as well if it is
        /// ready.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event arguments.</param>
        private void OnExitTransitionCompleted(object sender, EventArgs e)
        {
            _readyToTransitionToNewContent = true;
            _performingExitTransition = false;

            CompleteTransition(_storedNavigationOutTransition, /*_oldContentPresenter*/ null, _storedOldTransition);
            _storedNavigationOutTransition = null;
            _storedOldTransition = null;

            if (_contentReady)
            {
                ITransition newTransition = _storedNewTransition;
                NavigationInTransition navigationInTransition = _storedNavigationInTransition;

                _storedNewTransition = null;
                _storedNavigationInTransition = null;

                TransitionNewContent(newTransition, navigationInTransition);
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application 
        /// code or internal processes (such as a rebuilding layout pass) call
        /// <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// In simplest terms, this means the method is called just before a UI 
        /// element displays in an application.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _firstContentPresenter = GetTemplateChild(FirstTemplatePartName) as ContentPresenter;
            _secondContentPresenter = GetTemplateChild(SecondTemplatePartName) as ContentPresenter;
            _newContentPresenter = _secondContentPresenter;
            _oldContentPresenter = _firstContentPresenter;
            _useFirstAsNew = true;

            _readyToTransitionToNewContent = true;

            if (Content != null)
            {
                OnContentChanged(null, Content);
            }
        }

        /// <summary>
        /// Called when the value of the
        /// <see cref="P:System.Windows.Controls.ContentControl.Content"/>
        /// property changes.
        /// </summary>
        /// <param name="oldContent">The old <see cref="T:System.Object"/>.</param>
        /// <param name="newContent">The new <see cref="T:System.Object"/>.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            _contentReady = true;

            UIElement oldElement = oldContent as UIElement;
            UIElement newElement = newContent as UIElement;

            // Require the appropriate template parts plus a new element to
            // transition to.
            if (_firstContentPresenter == null || _secondContentPresenter == null || newElement == null)
            {
                return;
            }

            NavigationInTransition navigationInTransition = null;
            ITransition newTransition = null;

            if (newElement != null)
            {
                navigationInTransition = TransitionService.GetNavigationInTransition(newElement);
                TransitionElement newTransitionElement = null;
                if (navigationInTransition != null)
                {
                    newTransitionElement = _isForwardNavigation ? navigationInTransition.Forward : navigationInTransition.Backward;
                }
                if (newTransitionElement != null)
                {
                    newElement.UpdateLayout();

                    newTransition = newTransitionElement.GetTransition(newElement);
                    PrepareContentPresenterForCompositor(_newContentPresenter);
                }
            }

            _newContentPresenter.Opacity = 0;
            _newContentPresenter.Visibility = Visibility.Visible;
            _newContentPresenter.Content = newElement;

            _oldContentPresenter.Opacity = 1;
            _oldContentPresenter.Visibility = Visibility.Visible;
            _oldContentPresenter.Content = oldElement;

            if (_readyToTransitionToNewContent)
            {
                TransitionNewContent(newTransition, navigationInTransition);
            }
            else
            {
                _storedNewTransition = newTransition;
                _storedNavigationInTransition = navigationInTransition;
            }
        }

        /// <summary>
        /// Handles the BackKeyPress to stop the animation and go back.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event arguments.</param>
        private void OnBackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // No need to handle backkeypress if exit transition is complete.
            if (_performingExitTransition)
            {
                var oldElement = Content as UIElement;
                if (oldElement == null)
                {
                    return;
                }

                TransitionElement oldTransitionElement = null;
                NavigationOutTransition navigationOutTransition = null;
                ITransition oldTransition = null;

                navigationOutTransition = TransitionService.GetNavigationOutTransition(oldElement);

                if (navigationOutTransition != null)
                {
                    oldTransitionElement = _isForwardNavigation ? navigationOutTransition.Forward : navigationOutTransition.Backward;
                }
                if (oldTransitionElement != null)
                {
                    oldTransition = oldTransitionElement.GetTransition(oldElement);
                }
                if (oldTransition != null)
                {
                    CompleteTransition(_storedNavigationOutTransition, /*_oldContentPresenter*/ null, _storedOldTransition);
                    TransitionNewContent(oldTransition, null);
                }
            }
        }

        /// <summary>
        /// Transitions the new <see cref="T:System.Windows.UIElement"/>.
        /// </summary>
        /// <param name="newTransition">The <see cref="T:Microsoft.Phone.Controls.ITransition"/> 
        /// for the new <see cref="T:System.Windows.UIElement"/>.</param>
        /// <param name="navigationInTransition">The <see cref="T:Microsoft.Phone.Controls.NavigationInTransition"/> 
        /// for the new <see cref="T:System.Windows.UIElement"/>.</param>
        private void TransitionNewContent(ITransition newTransition, NavigationInTransition navigationInTransition)
        {
            if (_oldContentPresenter != null)
            {
                _oldContentPresenter.Visibility = Visibility.Collapsed;
                _oldContentPresenter.Content = null;
            }

            if (null == newTransition)
            {
                RestoreContentPresenterInteractivity(_newContentPresenter);
                return;
            }

            EnsureStoppedTransition(newTransition);
            newTransition.Completed += delegate
            {
                CompleteTransition(navigationInTransition, _newContentPresenter, newTransition);
            };

            _readyToTransitionToNewContent = false;
            _storedNavigationInTransition = null;
            _storedNewTransition = null;

            PerformTransition(navigationInTransition, _newContentPresenter, newTransition);
        }

        /// <summary>
        /// This checks to make sure that, if the transition not be in the clock
        /// state of Stopped, that is will be stopped.
        /// </summary>
        /// <param name="transition">The transition instance.</param>
        private static void EnsureStoppedTransition(ITransition transition)
        {
            if (transition != null && transition.GetCurrentState() != ClockState.Stopped)
            {
                transition.Stop();
            }
        }

        /// <summary>
        /// Performs a transition when given the appropriate components,
        /// includes calling the appropriate start event and ensuring opacity
        /// on the content presenter.
        /// </summary>
        /// <param name="navigationTransition">The navigation transition.</param>
        /// <param name="presenter">The content presenter.</param>
        /// <param name="transition">The transition instance.</param>
        private static void PerformTransition(NavigationTransition navigationTransition, ContentPresenter presenter, ITransition transition)
        {
            if (navigationTransition != null)
            {
                navigationTransition.OnBeginTransition();
            }
            if (presenter != null && presenter.Opacity != 1)
            {
                presenter.Opacity = 1;
            }
            if (transition != null)
            {
                transition.Begin();
            }
        }

        /// <summary>
        /// Completes a transition operation by stopping it, restoring 
        /// interactivity, and then firing the OnEndTransition event.
        /// </summary>
        /// <param name="navigationTransition">The navigation transition.</param>
        /// <param name="presenter">The content presenter.</param>
        /// <param name="transition">The transition instance.</param>
        private static void CompleteTransition(NavigationTransition navigationTransition, ContentPresenter presenter, ITransition transition)
        {
            if (transition != null)
            {
                transition.Stop();
            }

            RestoreContentPresenterInteractivity(presenter);

            if (navigationTransition != null)
            {
                navigationTransition.OnEndTransition();
            }
        }

        /// <summary>
        /// Updates the content presenter for off-thread compositing for the
        /// transition animation. Also disables interactivity on it to prevent
        /// accidental touches.
        /// </summary>
        /// <param name="presenter">The content presenter instance.</param>
        /// <param name="applyBitmapCache">A value indicating whether to apply
        /// a bitmap cache.</param>
        private static void PrepareContentPresenterForCompositor(ContentPresenter presenter, bool applyBitmapCache = true)
        {
            if (presenter != null)
            {
                if (applyBitmapCache)
                {
                    presenter.CacheMode = BitmapCacheMode;
                }
                presenter.IsHitTestVisible = false;
            }
        }

        /// <summary>
        /// Restores the interactivity for the presenter post-animation, also
        /// removes the BitmapCache value.
        /// </summary>
        /// <param name="presenter">The content presenter instance.</param>
        private static void RestoreContentPresenterInteractivity(ContentPresenter presenter)
        {
            if (presenter != null)
            {
                presenter.CacheMode = null;

                if (presenter.Opacity != 1)
                {
                    presenter.Opacity = 1;
                }

                presenter.IsHitTestVisible = true;
            }
        }
    }
}
