using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Coding4Fun.Phone.Controls
{
    public class ProgressOverlay : Control
    {
        private Storyboard fadeIn;
        private Storyboard fadeOut;
        private Grid LayoutGrid;
        
        private const string fadeInName = "fadeIn";
        private const string fadeOutName = "fadeOut";
        private const string LayoutGridName = "LayoutGrid";
        
        private bool _hasHookedUpGestureWatcher = false;

        public ProgressOverlay()
        {
            DefaultStyleKey = typeof(ProgressOverlay);
        }

        public object ProgressControl
        {
            get { return (object)GetValue(ProgressControlProperty); }
            set { SetValue(ProgressControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressControlProperty =
            DependencyProperty.Register("ProgressControl", typeof(object), typeof(ProgressOverlay), new PropertyMetadata(null));

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(ProgressOverlay), new PropertyMetadata(null));

        public bool HasGesturesDisabled
        {
            get { return (bool)GetValue(HasGesturesDisabledProperty); }
            set { SetValue(HasGesturesDisabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasGesturesDisabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasGesturesDisabledProperty =
            DependencyProperty.Register("HasGesturesDisabled", typeof(bool), typeof(ProgressOverlay), new PropertyMetadata(true, OnHasGesturesDisabledChanged));

        private static void OnHasGesturesDisabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = ((ProgressOverlay)o);
            if (sender != null && e.NewValue != e.OldValue)
                GestureHelper.WireUpGestureEvents((bool)e.NewValue, sender);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!_hasHookedUpGestureWatcher)
            {
                GestureHelper.WireUpGestureEvents(HasGesturesDisabled, this);
                _hasHookedUpGestureWatcher = true;
            }

            fadeIn = GetTemplateChild(fadeInName) as Storyboard;
            fadeOut = GetTemplateChild(fadeOutName) as Storyboard;
            LayoutGrid = GetTemplateChild(LayoutGridName) as Grid;
            
            if(fadeOut != null)
                fadeOut.Completed += fadeOut_Completed;
        }

        void fadeOut_Completed(object sender, EventArgs e)
        {
            LayoutGrid.Opacity = 1;
            Visibility = Visibility.Collapsed;
        }

        public void Show()
        {
            if (fadeIn == null)
                ApplyTemplate();
            
            Visibility = Visibility.Visible;
            
            if (fadeOut != null)
                fadeOut.Stop();

            if (fadeIn != null)
                fadeIn.Begin();
        }

        public void Hide()
        {
            if (fadeOut == null)
                ApplyTemplate();

            if (fadeIn != null)
                fadeIn.Stop();

            if (fadeOut != null) 
                fadeOut.Begin();
        }
    }
}
