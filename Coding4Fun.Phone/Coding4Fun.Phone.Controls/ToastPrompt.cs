using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Coding4Fun.Phone.Controls
{
    public class ToastPrompt : PopUp<string, PopUpResult>
    {
        protected Image ToastImage;
        private const string ToastImageName = "ToastImage";
        private DispatcherTimer _timer;
        private TranslateTransform _translate = new TranslateTransform();
        public ToastPrompt()
        {
            DefaultStyleKey = typeof(ToastPrompt);
            Overlay = (Brush)Application.Current.Resources["TransparentBrush"];
            AnimationType = Clarity.Phone.Extensions.DialogService.AnimationTypes.SlideHorizontal;
            HasGesturesDisabled = false;
            ManipulationStarted += ToastPrompt_ManipulationStarted;
            ManipulationDelta += ToastPrompt_ManipulationDelta;
            ManipulationCompleted += ToastPrompt_ManipulationCompleted;

            RenderTransform = _translate;
        }

        void ToastPrompt_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (e.TotalManipulation.Translation.X > 200 ||
                e.FinalVelocities.LinearVelocity.X > 1000)
                OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.Ok });
            else
            {
                _translate.X = 0;
                if (_timer != null)
                    _timer.Start();
            }
        }

        void ToastPrompt_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            _translate.X += e.DeltaManipulation.Translation.X;
            if (_translate.X < 0)
                _translate.X = 0;
        }

        void ToastPrompt_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (_timer != null)
                _timer.Stop();
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
            base.Show();

            if (!IsTimerEnabled)
                return;

            _timer = new DispatcherTimer
                         {
                             Interval = TimeSpan.FromMilliseconds(MillisecondsUntilHidden)
                         };

            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            DestroyTimer();
            OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.NoResponse });
        }

        protected override void OnCompleted(PopUpEventArgs<string, PopUpResult> result)
        {
            DestroyTimer();
            base.OnCompleted(result);
        }

        private void DestroyTimer()
        {
            if (_timer == null)
                return;

            _timer.Stop();
            _timer = null;
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