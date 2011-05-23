using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coding4Fun.Phone.Controls
{
    public class ToastPrompt : PopUp<string, PopUpResult>
    {
        protected Image ToastImage;
        private const string ToastImageName = "ToastImage";
        readonly Timer _timer;

        private readonly TranslateTransform _translate = new TranslateTransform();
		
        public ToastPrompt()
        {
            DefaultStyleKey = typeof(ToastPrompt);
			
			IsAppBarVisible = true;
            IsBackKeyOverride = true;
            Overlay = (Brush)Application.Current.Resources["TransparentBrush"];
            AnimationType = Clarity.Phone.Extensions.DialogService.AnimationTypes.SlideHorizontal;
            HasGesturesDisabled = false;

            ManipulationStarted += ToastPrompt_ManipulationStarted;
            ManipulationDelta += ToastPrompt_ManipulationDelta;
            ManipulationCompleted += ToastPrompt_ManipulationCompleted;

            RenderTransform = _translate;

            _timer = new Timer(_timer_Tick);
        }

        void ToastPrompt_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (e.TotalManipulation.Translation.X > 200 || e.FinalVelocities.LinearVelocity.X > 1000)
                OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.UserDismissed });
            else if (e.TotalManipulation.Translation.X < 20)
            {
                OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.Ok });
            }
            else
            {
                _translate.X = 0;
                StartTimer();  
            }
        }

        void ToastPrompt_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            _translate.X += e.DeltaManipulation.Translation.X;
            if (_translate.X < 0)
                _translate.X = 0;
        }

        private void StartTimer()
        {
            if (_timer != null)
                _timer.Change(TimeSpan.FromMilliseconds(MillisecondsUntilHidden), TimeSpan.FromMilliseconds(-1));
        }

        private void PauseTimer()
        {
            if (_timer != null)
                _timer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));
        }


        void ToastPrompt_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            PauseTimer();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ToastImage = GetTemplateChild(ToastImageName) as Image;

            if (ToastImage != null && ImageSource != null)
            {
                ToastImage.Source = ImageSource;
                SetImageVisibility(ImageSource);
            }
        }

        public override void Show()
        {
            if (!IsTimerEnabled)
                return;

            StartTimer();

            base.Show();
        }

        void _timer_Tick(object state)
        {
            Dispatcher.BeginInvoke(() => OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.NoResponse }));
        }

        public override void OnCompleted(PopUpEventArgs<string, PopUpResult> result)
        {
            PauseTimer();
        
            base.OnCompleted(result);
        }

        public int MillisecondsUntilHidden
        {
            get { return (int)GetValue(MillisecondsUntilHiddenProperty); }
            set { SetValue(MillisecondsUntilHiddenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MillisecondsUntilHidden.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MillisecondsUntilHiddenProperty =
            DependencyProperty.Register("MillisecondsUntilHidden", typeof(int), typeof(ToastPrompt), new PropertyMetadata(4000));

        public bool IsTimerEnabled
        {
            get { return (bool)GetValue(IsTimerEnabledProperty); }
            set { SetValue(IsTimerEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTimerEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTimerEnabledProperty =
            DependencyProperty.Register("IsTimerEnabled", typeof(bool), typeof(ToastPrompt), new PropertyMetadata(true));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ToastPrompt), new PropertyMetadata(""));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ToastPrompt), new PropertyMetadata(""));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ToastPrompt),
            new PropertyMetadata(OnImageSource));
        
        public Orientation TextOrientation
        {
            get { return (Orientation)GetValue(TextOrientationProperty); }
            set { SetValue(TextOrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextOrientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextOrientationProperty =
            DependencyProperty.Register("TextOrientation", typeof(Orientation), typeof(ToastPrompt), new PropertyMetadata(Orientation.Horizontal));

        private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as ToastPrompt;

            if (sender == null || sender.ToastImage == null)
                return;

            sender.SetImageVisibility(e.NewValue as ImageSource);
        }

        private void SetImageVisibility(ImageSource source)
        {
            ToastImage.Visibility = (source == null) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}