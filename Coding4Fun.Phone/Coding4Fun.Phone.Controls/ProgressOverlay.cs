using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

using Coding4Fun.Phone.Controls.Helpers;

namespace Coding4Fun.Phone.Controls
{
    [ContentProperty("Content")]
    public class ProgressOverlay : Control
    {
        private Storyboard _fadeIn;
        private Storyboard _fadeOut;
        private Grid _layoutGrid;
        
        private const string FadeInName = "fadeIn";
        private const string FadeOutName = "fadeOut";
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

            _fadeIn = GetTemplateChild(FadeInName) as Storyboard;
            _fadeOut = GetTemplateChild(FadeOutName) as Storyboard;
            _layoutGrid = GetTemplateChild(LayoutGridName) as Grid;
            
            if(_fadeOut != null)
                _fadeOut.Completed += fadeOut_Completed;
        }

        void fadeOut_Completed(object sender, EventArgs e)
        {
            _layoutGrid.Opacity = 1;
            Visibility = Visibility.Collapsed;
        }

        public void Show()
        {
            if (_fadeIn == null)
                ApplyTemplate();
            
            Visibility = Visibility.Visible;
            
            if (_fadeOut != null)
                _fadeOut.Stop();

            if (_fadeIn != null)
                _fadeIn.Begin();
        }

        public void Hide()
        {
            if (_fadeOut == null)
                ApplyTemplate();

            if (_fadeIn != null)
                _fadeIn.Stop();

            if (_fadeOut != null) 
                _fadeOut.Begin();
        }
    }
}
