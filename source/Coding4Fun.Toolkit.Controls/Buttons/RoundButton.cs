﻿#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	public partial class RoundButton : ButtonBase, IAppBarButton
    {
		public RoundButton()
		{
			DefaultStyleKey = typeof(RoundButton);
		}

		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);

			if (oldContent == newContent)
				return;

			AppendCheck(Content);

			// content changed, wait a tick
#if WINDOWS_STORE || WINDOWS_PHONE_APP
			ButtonBaseHelper.ApplyForegroundToFillBinding(
				GetTemplateChild(ButtonBaseConstants.ContentBodyName) as ContentControl);
#elif WINDOWS_PHONE
			Dispatcher.BeginInvoke(() =>
								   ButtonBaseHelper.ApplyForegroundToFillBinding(
									   GetTemplateChild(ButtonBaseConstants.ContentBodyName) as ContentControl));
#endif
		}

		private void AppendCheck(object content)
		{
			if (!IsContentEmpty(content))
				return;

			Content = ButtonBaseHelper.CreateXamlCheck(this);
		}

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
        {
			ApplyingTemplate();

			AppendCheck(Content);

			ButtonBaseHelper.ApplyForegroundToFillBinding(GetTemplateChild(ButtonBaseConstants.ContentBodyName) as ContentControl);
			ButtonBaseHelper.ApplyTitleOffset(GetTemplateChild(ButtonBaseConstants.ContentTitleName) as ContentControl);

			base.OnApplyTemplate();
        }
        #region dependency properties

		public Brush PressedBrush
		{
			get { return (Brush)GetValue(PressedBrushProperty); }
			set { SetValue(PressedBrushProperty, value); }
		}

		// Using a DependencyProperty as the backing store for PressedBrush.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty PressedBrushProperty =
			DependencyProperty.Register("PressedBrush", typeof(Brush), typeof(RoundButton), new PropertyMetadata(default(SolidColorBrush)));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RoundButton), new PropertyMetadata(Orientation.Vertical));

		public double ButtonWidth
		{
			get { return (double)GetValue(ButtonWidthProperty); }
			set { SetValue(ButtonWidthProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonWidth.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonWidthProperty =
			DependencyProperty.Register("ButtonWidth", typeof(double), typeof(RoundButton), new PropertyMetadata(double.NaN));

		public double ButtonHeight
		{
			get { return (double)GetValue(ButtonHeightProperty); }
			set { SetValue(ButtonHeightProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonHeight.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonHeightProperty =
			DependencyProperty.Register("ButtonHeight", typeof(double), typeof(RoundButton), new PropertyMetadata(double.NaN));
        #endregion
    }
}