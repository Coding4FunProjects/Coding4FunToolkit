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

        public ProgressOverlay()
        {
            DefaultStyleKey = typeof(ProgressOverlay);
            DataContext = this;
        }

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(ProgressOverlay), new PropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            fadeIn = GetTemplateChild(fadeInName) as Storyboard;
            fadeOut = GetTemplateChild(fadeOutName) as Storyboard;
            LayoutGrid = GetTemplateChild(LayoutGridName) as Grid;
        }

        public void Show()
        {
            if (fadeIn == null)
                ApplyTemplate();

            if (Visibility == Visibility.Collapsed)
            {
                LayoutGrid.Opacity = 0;
                Visibility = Visibility.Visible;
            }

            if (fadeIn != null) 
                fadeIn.Begin();
        }

        public void Hide()
        {
            if (fadeOut == null)
                ApplyTemplate();

            if (fadeOut != null) 
                fadeOut.Begin();
        }
    }
}
