using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Clarity.Phone.Extensions
{
    // this code has been modified from the orginal code
    // from Kevin Marshall's post 
    // http://blogs.claritycon.com/kevinmarshall/2010/10/13/wp7-page-transitions-sample/

    public class DialogService
    {
        public enum AnimationTypes
        {
            Slide,
            SlideHorizontal,
            Swivel,
            SwivelHorizontal
        }

        private const string SlideUpStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.Y)"" 
                                           Storyboard.TargetName=""LayoutRoot"">
                    <EasingDoubleKeyFrame KeyTime=""0"" Value=""150""/>
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                        <EasingDoubleKeyFrame.EasingFunction>
                            <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                        </EasingDoubleKeyFrame.EasingFunction>
                    </EasingDoubleKeyFrame>
                </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""0"" To=""1"" Duration=""0:0:0.350"" 
                                 Storyboard.TargetName=""LayoutRoot"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SlideHorizontalInStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.X)"" 
                                           Storyboard.TargetName=""LayoutRoot"">
                    <EasingDoubleKeyFrame KeyTime=""0"" Value=""-150""/>
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                        <EasingDoubleKeyFrame.EasingFunction>
                            <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                        </EasingDoubleKeyFrame.EasingFunction>
                    </EasingDoubleKeyFrame>
                </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""0"" To=""1"" Duration=""0:0:0.350"" 
                                 Storyboard.TargetName=""LayoutRoot"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SlideHorizontalOutStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.X)"" 
                                           Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""150"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""1"" To=""0"" Duration=""0:0:0.25"" 
                                 Storyboard.TargetName=""LayoutRoot"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SlideDownStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.Y)"" 
                                           Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""150"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""1"" To=""0"" Duration=""0:0:0.25"" 
                                 Storyboard.TargetName=""LayoutRoot"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SwivelInStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation BeginTime=""0:0:0"" Duration=""0"" 
                                Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.CenterOfRotationY)"" 
                                Storyboard.TargetName=""LayoutRoot""
                                To="".5""/>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"" Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""-30""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)""
                                            Storyboard.TargetName=""LayoutRoot"">
                <DiscreteDoubleKeyFrame KeyTime=""0"" Value=""1"" />
            </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        private const string SwivelOutStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation BeginTime=""0:0:0"" Duration=""0"" 
                                Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.CenterOfRotationY)"" 
                                Storyboard.TargetName=""LayoutRoot""
                                To="".5""/>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"" Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""45"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)""
                                            Storyboard.TargetName=""LayoutRoot"">
                <DiscreteDoubleKeyFrame KeyTime=""0"" Value=""1"" />
                <DiscreteDoubleKeyFrame KeyTime=""0:0:0.267"" Value=""0"" />
            </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        private Panel _popupContainer;
        private Frame _rootVisual;
        private PhoneApplicationPage _page;
        private IApplicationBar _originalAppBar;
        private Panel _overlay;
        private Storyboard _showStoryboard;
        private Storyboard _hideStoryboard;

        public FrameworkElement Child { get; set; }
        public AnimationTypes AnimationType { get; set; }
        public double VerticalOffset { get; set; }
        public Brush BackgroundBrush { get; set; }

        internal IApplicationBar AppBar { get; set; }
        internal bool IsOpen { get; set; }
        protected internal bool IsBackKeyOverride { get; set; }

        public event EventHandler Closed;
        public event EventHandler Opened;

        // set this to prevent the dialog service from closing on back click
        public bool HasPopup { get; set; }

        internal PhoneApplicationPage Page
        {
            get { return _page ?? (_page = RootVisual.GetVisualDescendants().OfType<PhoneApplicationPage>().FirstOrDefault()); }
        }

        internal Frame RootVisual
        {
            get { return _rootVisual ?? (_rootVisual = Application.Current.RootVisual as Frame); }
        }

        internal Panel PopupContainer
        {
            get
            {
                if (_popupContainer == null)
                {
                    var presenters = RootVisual.GetVisualDescendants().OfType<ContentPresenter>();
                    for (var i = 0; i < presenters.Count(); i++)
                    {

                        var panels = presenters.ElementAt(i).GetVisualDescendants().OfType<Panel>();

                        if (panels.Count() <= 0)
                            continue;
                        _popupContainer = panels.First();
                        break;
                    }
                }


                return _popupContainer;
            }
        }

        public DialogService()
        {
            AnimationType = AnimationTypes.Slide;
        }

        private void InitializePopup()
        {
            // Add overlay which is the size of RootVisual
            _overlay = new Grid();

            Grid.SetColumnSpan(_overlay, int.MaxValue);
            Grid.SetRowSpan(_overlay, int.MaxValue);

            switch (AnimationType)
            {
                case AnimationTypes.SlideHorizontal:
                    _showStoryboard = XamlReader.Load(SlideHorizontalInStoryboard) as Storyboard;
                    _hideStoryboard = XamlReader.Load(SlideHorizontalOutStoryboard) as Storyboard;
                    _overlay.RenderTransform = new TranslateTransform();
                    break;

                case AnimationTypes.Slide:
                    _showStoryboard = XamlReader.Load(SlideUpStoryboard) as Storyboard;
                    _hideStoryboard = XamlReader.Load(SlideDownStoryboard) as Storyboard;
                    _overlay.RenderTransform = new TranslateTransform();
                    break;

                default:
                    _showStoryboard = XamlReader.Load(SwivelInStoryboard) as Storyboard;
                    _hideStoryboard = XamlReader.Load(SwivelOutStoryboard) as Storyboard;
                    _overlay.Projection = new PlaneProjection();
                    break;
            }

            _overlay.Children.Add(Child);

            if (BackgroundBrush != null)
                _overlay.Background = BackgroundBrush;

			_overlay.Margin = new Thickness(0, VerticalOffset, 0, 0);
            _overlay.Opacity = 0;

            // Initialize popup to draw the context menu over all controls
            PopupContainer.Children.Add(_overlay);
        }

        protected internal void SetAlignmentsOnOverlay(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            if (_overlay != null)
            {
                _overlay.HorizontalAlignment = horizontalAlignment;
                _overlay.VerticalAlignment = verticalAlignment;
            }
        }

        /// <summary>
        /// Shows the context menu.
        /// </summary>
        public void Show()
        {
            IsOpen = true;

            InitializePopup();

            if(!IsBackKeyOverride)
                Page.BackKeyPress += OnBackKeyPress;

            Page.NavigationService.Navigated += OnNavigated;

            _originalAppBar = Page.ApplicationBar;

            _showStoryboard.Completed += _showStoryboard_Completed;

            foreach (Timeline t in _showStoryboard.Children)
                Storyboard.SetTarget(t, _overlay);

            PopupContainer.InvokeOnLayoutUpdated(() =>
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _showStoryboard.Begin();

                    if (Page != null)
                    {
                        Page.ApplicationBar = AppBar;
                    }
                }));
        }

        void _showStoryboard_Completed(object sender, EventArgs e)
        {
            var sb = sender as Storyboard;

            if (sb != null)
                sb.Completed -= _showStoryboard_Completed;

            if (Opened != null)
                Opened(this, null);
        }

        void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Hide();
        }

        public void Hide()
        {
            if (!IsOpen)
                return;

            if (Page != null)
            {
                Page.BackKeyPress -= OnBackKeyPress;
                Page.NavigationService.Navigated -= OnNavigated;

                if (_originalAppBar != null)
                    Page.ApplicationBar = _originalAppBar;
                //else
                //    Page.ApplicationBar = null;

                _page = null;
            }

            if (_hideStoryboard != null)
            {
                _hideStoryboard.Stop();

                foreach (Timeline t in _hideStoryboard.Children)
                {
                    Storyboard.SetTarget(t, _overlay);
                }

                _hideStoryboard.Completed += _hideStoryboard_Completed;
                _hideStoryboard.Begin();
            }
        }

        void _hideStoryboard_Completed(object sender, EventArgs e)
        {
            if (_hideStoryboard != null)
            {
                _hideStoryboard.Completed -= _hideStoryboard_Completed;
                _hideStoryboard = null;
            }

            IsOpen = false;

            if (PopupContainer != null)
            {
                PopupContainer.Children.Remove(_overlay);
            }

            if (null != _overlay)
            {
                _overlay.Children.Clear();
                _overlay = null;
            }

            if (Closed != null)
                Closed(this, null);
        }

        public void OnBackKeyPress(object sender, CancelEventArgs e)
        {
            if (HasPopup)
            {
                e.Cancel = true;
                return;
            }

            if (IsOpen)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}