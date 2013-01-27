#if WINDOWS_STORE
using System;
using Windows.UI.Core;
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
	public abstract partial class ToggleButtonBase : CheckBox, IButtonBase, IAppBarButton
	{
		protected ToggleButtonBase()
		{
			IsEnabledChanged += IsEnabledStateChanged;
		}

		void IsEnabledStateChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			IsEnabledStateChanged();
		}

		private void IsEnabledStateChanged()
		{
			var contentBody = GetTemplateChild(ButtonBaseConstants.ContentBodyName) as ContentControl;
			var enabledContentControl = GetTemplateChild(ButtonBaseConstants.EnabledContentControlName) as ContentControl;
			var disabledContentControl = GetTemplateChild(ButtonBaseConstants.DisabledContentControlName) as ContentControl;

			if (contentBody != null)
			{
				var content = contentBody.Parent as ContentControl;
				
				if(content != null)
					content.Content = null;
			}

			if (IsEnabled)
			{
				if (enabledContentControl != null) 
					enabledContentControl.Content = contentBody;
			}
			else
			{
				if (disabledContentControl != null)
					disabledContentControl.Content = contentBody;
			}

#if WINDOWS_STORE
			Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
#elif WINDOWS_PHONE
			Dispatcher.BeginInvoke(
#endif
			                    () => ButtonBaseHelper.ApplyForegroundToFillBinding(contentBody));
		}

		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);

			if (oldContent == newContent)
				return;

			AppendCheck(Content);
		}

		private void AppendCheck(object content)
		{
			if (!IsContentEmpty(content))
				return;

			Content = ButtonBaseHelper.CreateXamlCheck(this);
		}

#if WINDOWS_STORE
		protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
		{
			base.OnApplyTemplate();

			ApplyingTemplate();

			AppendCheck(Content);
			
			IsEnabledStateChanged();

			ButtonBaseHelper.ApplyTitleOffset(GetTemplateChild(ButtonBaseConstants.ContentTitleName) as ContentControl);
		}

		#region dependency properties

		public object Label
		{
			get { return GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register("Label", typeof (object), typeof (ToggleButtonBase), 
				new PropertyMetadata(string.Empty));

		public Brush CheckedBrush
		{
			get { return (Brush) GetValue(CheckedBrushProperty); }
			set { SetValue(CheckedBrushProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CheckedBrush.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CheckedBrushProperty =
			DependencyProperty.Register("CheckedBrush", typeof (Brush), typeof (ToggleButtonBase),
				new PropertyMetadata(new SolidColorBrush()));

		public Orientation Orientation
		{
			get { return (Orientation) GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty OrientationProperty =
			DependencyProperty.Register("Orientation", typeof (Orientation), typeof (ToggleButtonBase),
				new PropertyMetadata(Orientation.Vertical));

		public double ButtonWidth
		{
			get { return (double) GetValue(ButtonWidthProperty); }
			set { SetValue(ButtonWidthProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonWidth.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonWidthProperty =
			DependencyProperty.Register("ButtonWidth", typeof (double), typeof (ToggleButtonBase), 
				new PropertyMetadata(double.NaN));

		public double ButtonHeight
		{
			get { return (double) GetValue(ButtonHeightProperty); }
			set { SetValue(ButtonHeightProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonHeight.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonHeightProperty =
			DependencyProperty.Register("ButtonHeight", typeof (double), typeof (ToggleButtonBase), 
				new PropertyMetadata(double.NaN));

		#endregion
	}
}