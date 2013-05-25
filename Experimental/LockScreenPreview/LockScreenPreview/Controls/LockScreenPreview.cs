using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Coding4Fun.Toolkit.Controls
{
    public class LockScreenPreview : Control
    {
        private const string ContainingGrid = "ContainingGrid";

        private Grid _containingGrid;

        public static readonly DependencyProperty LockScreenImageSourceProperty = DependencyProperty.Register(
            "LockScreenImageSource",
            typeof (ImageSource),
            typeof (LockScreenPreview),
            new PropertyMetadata(default(ImageSource)));

        public ImageSource LockScreenImageSource
        {
            get { return (ImageSource) GetValue(LockScreenImageSourceProperty); }
            set { SetValue(LockScreenImageSourceProperty, value); }
        }

        public static readonly DependencyProperty TextLine1Property = DependencyProperty.Register(
            "TextLine1",
            typeof (string),
            typeof (LockScreenPreview),
            new PropertyMetadata(default(string)));

        public string TextLine1
        {
            get { return (string) GetValue(TextLine1Property); }
            set { SetValue(TextLine1Property, value); }
        }

        public static readonly DependencyProperty TextLine2Property = DependencyProperty.Register(
            "TextLine2",
            typeof (string),
            typeof (LockScreenPreview),
            new PropertyMetadata(default(string)));

        public string TextLine2
        {
            get { return (string) GetValue(TextLine2Property); }
            set { SetValue(TextLine2Property, value); }
        }

        public static readonly DependencyProperty TextLine3Property = DependencyProperty.Register(
            "TextLine3",
            typeof (string),
            typeof (LockScreenPreview),
            new PropertyMetadata(default(string)));

        public string TextLine3
        {
            get { return (string) GetValue(TextLine3Property); }
            set { SetValue(TextLine3Property, value); }
        }

        public static readonly DependencyProperty NotificationIconSourceProperty = DependencyProperty.Register(
            "NotificationIconSource",
            typeof (ImageSource), 
            typeof (LockScreenPreview), 
            new PropertyMetadata(default(ImageSource)));

        public ImageSource NotificationIconSource
        {
            get { return (ImageSource) GetValue(NotificationIconSourceProperty); }
            set { SetValue(NotificationIconSourceProperty, value); }
        }

        public static readonly DependencyProperty ShowNotificationCountProperty = DependencyProperty.Register(
            "ShowNotificationCount",
            typeof (bool),
            typeof (LockScreenPreview),
            new PropertyMetadata(true));

        public bool ShowNotificationCount
        {
            get { return (bool) GetValue(ShowNotificationCountProperty); }
            set { SetValue(ShowNotificationCountProperty, value); }
        }

        public static readonly DependencyProperty Support720Property = DependencyProperty.Register(
            "Support720", 
            typeof (bool), 
            typeof (LockScreenPreview), 
            new PropertyMetadata(default(bool)));

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LockScreenPreview"/> should display the image as if 
        /// it's on a 720p device.
        /// </summary>
        /// <value>
        ///   <c>true</c> if support720; otherwise, <c>false</c>.
        /// </value>
        public bool Support720
        {
            get { return (bool) GetValue(Support720Property); }
            set { SetValue(Support720Property, value); }
        }

        public LockScreenPreview()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //TextLine1 = "Calendar event";
                //TextLine2 = "With a location";
                //TextLine3 = "07:00 - 08:00";
            }

            DefaultStyleKey = typeof(LockScreenPreview);
        }

        public override void OnApplyTemplate()
        {
            _containingGrid = GetTemplateChild(ContainingGrid) as Grid;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// To the writeable bitmap.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The main containing grid could not be found in the template</exception>
        public WriteableBitmap ToWriteableBitmap()
        {
            if (_containingGrid == null)
            {
                throw new ArgumentNullException(ContainingGrid, "The main containing grid could not be found in the template");
            }

            var height = (int) Math.Floor(ActualHeight);
            var width = (int) Math.Floor(ActualWidth);

            var bitmap = new WriteableBitmap(width, height);

            bitmap.Render(_containingGrid, null);
            bitmap.Invalidate();

            return bitmap;
        }
    }
}