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
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.Y)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""150""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""0"" To=""1"" Duration=""0:0:0.350"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SlideHorizontalInStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.X)"" >
                    <EasingDoubleKeyFrame KeyTime=""0"" Value=""-150""/>
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                        <EasingDoubleKeyFrame.EasingFunction>
                            <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                        </EasingDoubleKeyFrame.EasingFunction>
                    </EasingDoubleKeyFrame>
                </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""0"" To=""1"" Duration=""0:0:0.350"" >
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SlideHorizontalOutStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.X)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""150"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""1"" To=""0"" Duration=""0:0:0.25"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SlideDownStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.Y)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""150"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""1"" To=""0"" Duration=""0:0:0.25"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SwivelInStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation BeginTime=""0:0:0"" Duration=""0"" To="".5""
                                Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.CenterOfRotationY)"" />
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""-30""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)"">
                <DiscreteDoubleKeyFrame KeyTime=""0"" Value=""1"" />
            </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        private const string SwivelOutStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation BeginTime=""0:0:0"" Duration=""0"" 
                                Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.CenterOfRotationY)"" 
                                To="".5""/>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""45"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)"">
                <DiscreteDoubleKeyFrame KeyTime=""0"" Value=""1"" />
                <DiscreteDoubleKeyFrame KeyTime=""0:0:0.267"" Value=""0"" />
            </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        private Panel _popupContainer;
        private Frame _rootVisual;
        private PhoneApplicationPage _page;
        private Panel _overlay;
        
        public FrameworkElement Child { get; set; }
        public AnimationTypes AnimationType { get; set; }
        public double VerticalOffset { get; set; }
        public Brush BackgroundBrush { get; set; }

        internal bool IsOpen { get; set; }
        protected internal bool IsBackKeyOverride { get; set; }

        public event EventHandler Closed;
        public event EventHandler Opened;

        // set this to prevent the dialog service from closing on back click
        public bool HasPopup { get; set; }

        internal PhoneApplicationPage Page
        {
            get { return _page ?? (_page = RootVisual.GetFirstLogicalChildByType<PhoneApplicationPage>(false)); }
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
                    var presenters = RootVisual.GetLogicalChildrenByType<ContentPresenter>(false);

                    for (var i = 0; i < presenters.Count(); i++)
                    {

						var panels = presenters.ElementAt(i).GetLogicalChildrenByType<Panel>(false);

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

		bool _deferredShowToLoaded;
		private void InitializePopup()
        {
            // Add overlay which is the size of RootVisual
            _overlay = new Grid {Name = Guid.NewGuid().ToString()};

			Grid.SetColumnSpan(_overlay, int.MaxValue);
			Grid.SetRowSpan(_overlay, int.MaxValue);

			if (BackgroundBrush != null)
				_overlay.Background = BackgroundBrush;

			if (SystemTray.IsVisible && SystemTray.Opacity < 1 && SystemTray.Opacity > 0)
			{
				VerticalOffset += 32;
			}

			_overlay.Margin = new Thickness(0, VerticalOffset, 0, 0);
			_overlay.Opacity = 0;

			// Initialize popup to draw the context menu over all controls
			if (PopupContainer != null)
			{
				PopupContainer.Children.Add(_overlay);
				_overlay.Children.Add(Child);
			}
			else
			{
				_deferredShowToLoaded = true;
				RootVisual.Loaded += RootVisualDeferredShow_Loaded;
			}
        }

		void RootVisualDeferredShow_Loaded(object sender, RoutedEventArgs e)
		{
			RootVisual.Loaded -= RootVisualDeferredShow_Loaded;
			_deferredShowToLoaded = false;
			Show();
		}

        protected internal void SetAlignmentsOnOverlay(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            if (_overlay != null)
            {
                _overlay.HorizontalAlignment = horizontalAlignment;
                _overlay.VerticalAlignment = verticalAlignment;
            }
        }

        private static readonly object Lockobj = new object();
        /// <summary>
        /// Shows the context menu.
        /// </summary>
        public void Show()
        {
            lock (Lockobj)
            {
                IsOpen = true;

                InitializePopup();

				if (_deferredShowToLoaded)
					return;

                if (!IsBackKeyOverride)
                    Page.BackKeyPress += OnBackKeyPress;

                Page.NavigationService.Navigated += OnNavigated;

                Storyboard storyboard;
                switch (AnimationType)
                {
                    case AnimationTypes.SlideHorizontal:
                        storyboard = XamlReader.Load(SlideHorizontalInStoryboard) as Storyboard;
                        _overlay.RenderTransform = new TranslateTransform();
                        break;

                    case AnimationTypes.Slide:
                        storyboard = XamlReader.Load(SlideUpStoryboard) as Storyboard;
                        _overlay.RenderTransform = new TranslateTransform();
                        break;

                    default:
                        storyboard = XamlReader.Load(SwivelInStoryboard) as Storyboard;
                        _overlay.Projection = new PlaneProjection();
                        break;
                } 
                
                if (storyboard != null)
                {
                    Page.Dispatcher.BeginInvoke(() =>
                                                    {

                                                        foreach (var t in storyboard.Children)
                                                            Storyboard.SetTarget(t, _overlay);

                                                        storyboard.Begin();

                                                    });
                }

				if (Opened != null)
					Opened.Invoke(this, null);
            }
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

                _page = null;
            }

            Storyboard storyboard;

            switch (AnimationType)
            {
                case AnimationTypes.SlideHorizontal:
                    storyboard = XamlReader.Load(SlideHorizontalOutStoryboard) as Storyboard;
                    break;

                case AnimationTypes.Slide:
                    storyboard = XamlReader.Load(SlideDownStoryboard) as Storyboard;
                    break;

                default:
                    storyboard = XamlReader.Load(SwivelOutStoryboard) as Storyboard;
                    break;
            }

            if (storyboard != null)
            {
                storyboard.Completed += _hideStoryboard_Completed;

                foreach (var t in storyboard.Children)
                    Storyboard.SetTarget(t, _overlay);

                storyboard.Begin();
            }
        }

        void _hideStoryboard_Completed(object sender, EventArgs e)
        {
            IsOpen = false;

            if (PopupContainer != null)
            {
                PopupContainer.Children.Remove(_overlay);
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