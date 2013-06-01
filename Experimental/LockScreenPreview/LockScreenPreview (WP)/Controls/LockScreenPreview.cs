using System;
using System.Globalization;

#if WINDOWS_STORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
#endif

namespace Coding4Fun.Toolkit.Controls
{
    [TemplatePart(Name = TimeText, Type=typeof(TextBlock))]
    [TemplatePart(Name = DayText, Type = typeof(TextBlock))]
    [TemplatePart(Name = DateText, Type = typeof(TextBlock))]
    public class LockScreenPreview : ContentControl
    {
        public const string TimeText = "TimeText";
        public const string DayText = "DayText";
        public const string DateText = "DateText";
		public const string LockScreenImage = "LockScreenImage";

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

#if WINDOWS_PHONE
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
#endif

        public LockScreenPreview()
        {
            DefaultStyleKey = typeof(LockScreenPreview);
        }

#if WINDOWS_STORE
        protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
        public override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();

			var now = DateTime.Now;
			var culture = CultureInfo.CurrentUICulture;
            
			var dateText = GetTemplateChild(DateText) as TextBlock;

			if (dateText != null)
            {
				dateText.Text = now.ToString(culture.DateTimeFormat.MonthDayPattern);
            }

			var dayText = GetTemplateChild(DayText) as TextBlock;

			if (dayText != null)
            {
				dayText.Text = now.DayOfWeek.ToString();
            }

			var timeText = GetTemplateChild(TimeText) as TextBlock;

			if (timeText != null)
            {
				timeText.Text = now.ToString(culture.DateTimeFormat.ShortTimePattern);
            }
        }
    }
}