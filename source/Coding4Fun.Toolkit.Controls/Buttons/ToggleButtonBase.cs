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
	public abstract partial class ToggleButtonBase : CheckBox, IButtonBase, IAppBarButton
	{

#if WINDOWS_STORE
		protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
		{
			base.OnApplyTemplate();

			ApplyingTemplate();
		}

		#region dependency properties

		public object Title
		{
			get { return GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof (object), typeof (ToggleButtonBase), new PropertyMetadata(new object()));

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
			DependencyProperty.Register("ButtonWidth", typeof (double), typeof (ToggleButtonBase), new PropertyMetadata(72d));

		public double ButtonHeight
		{
			get { return (double) GetValue(ButtonHeightProperty); }
			set { SetValue(ButtonHeightProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonHeight.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonHeightProperty =
			DependencyProperty.Register("ButtonHeight", typeof (double), typeof (ToggleButtonBase), new PropertyMetadata(72d));

		#endregion
	}
}