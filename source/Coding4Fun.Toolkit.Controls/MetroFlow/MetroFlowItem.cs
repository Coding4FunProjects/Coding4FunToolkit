using System.Windows;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

#elif WINDOWS_PHONE
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
#endif


namespace Coding4Fun.Toolkit.Controls
{
#if WINDOWS_STORE || WINDOWS_PHONE_APP
	[ContentProperty(Name = "Title")]
#elif WINDOWS_PHONE
    [ContentProperty("Title")]
#endif
    public class MetroFlowItem : Control
	{
		private const int DefaultStartIndex = 1;
		public MetroFlowItem()
		{
			DefaultStyleKey = typeof(MetroFlowItem);
		}

		#region ImageSource
		
		public ImageSource ImageSource
		{
			get { return (ImageSource)GetValue(ImageSourceProperty ); }
			set { SetValue(ImageSourceProperty , value); }
		}

		// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ImageSourceProperty  =
			DependencyProperty.Register(
                "ImageSource", 
                typeof(ImageSource), 
                typeof(MetroFlowItem), 
                new PropertyMetadata(
#if WINDOWS_STORE || WINDOWS_PHONE_APP
	                null,
#endif
                    OnImageSourceChanged
                ));

		private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var item = d as MetroFlowItem;

			if (item == null)
				return;

			item.UpdateLayout();
		}

		public Visibility ImageVisibility
		{
			get { return (Visibility)GetValue(ImageVisibilityProperty); }
			set { SetValue(ImageVisibilityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ImageVisibility.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ImageVisibilityProperty =
			DependencyProperty.Register("ImageVisibility", typeof(Visibility), typeof(MetroFlowItem), new PropertyMetadata(Visibility.Visible));

		public double ImageOpacity
		{
			get { return (double)GetValue(ImageOpacityProperty); }
			set { SetValue(ImageOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ImageOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ImageOpacityProperty =
					DependencyProperty.Register("ImageOpacity", typeof(double), typeof(MetroFlowItem), new PropertyMetadata(1d));
		#endregion
		#region ItemIndex
		public string ItemIndexString
		{
			get { return (string)GetValue(ItemIndexStringProperty); }
			private set { SetValue(ItemIndexStringProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ItemIndexString.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemIndexStringProperty =
			DependencyProperty.Register("ItemIndexString", typeof(string), typeof(MetroFlowItem), new PropertyMetadata(DefaultStartIndex.ToString()));

		public int ItemIndex
		{
			get { return (int)GetValue(ItemIndexProperty); }
			set 
			{ 
				SetValue(ItemIndexProperty, value);
				ItemIndexString = ItemIndex.ToString();
			}
		}

		// Using a DependencyProperty as the backing store for ItemIndex.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemIndexProperty =
			DependencyProperty.Register("ItemIndex", typeof(int), typeof(MetroFlowItem), new PropertyMetadata(DefaultStartIndex));

		public Visibility ItemIndexVisibility
		{
			get { return (Visibility)GetValue(ItemIndexVisibilityProperty); }
			set { SetValue(ItemIndexVisibilityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ItemIndexVisibility.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemIndexVisibilityProperty =
			DependencyProperty.Register("ItemIndexVisibility", typeof(Visibility), typeof(MetroFlowItem), new PropertyMetadata(Visibility.Collapsed));

		public double ItemIndexOpacity
		{
			get { return (double)GetValue(ItemIndexOpacityProperty); }
			set { SetValue(ItemIndexOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ItemIndexOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemIndexOpacityProperty =
			DependencyProperty.Register("ItemIndexOpacity", typeof(double), typeof(MetroFlowItem), new PropertyMetadata(0d));
		#endregion
		#region Title
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(MetroFlowItem), new PropertyMetadata("Lorem ipsum dolor sit amet"));

		public Visibility TitleVisibility
		{
			get { return (Visibility)GetValue(TitleVisibilityProperty); }
			set { SetValue(TitleVisibilityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for TitleVisibility.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleVisibilityProperty =
			DependencyProperty.Register("TitleVisibility", typeof(Visibility), typeof(MetroFlowItem), new PropertyMetadata(Visibility.Visible));

		public double TitleOpacity
		{
			get { return (double)GetValue(TitleOpacityProperty); }
			set { SetValue(TitleOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for TitleOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleOpacityProperty =
			DependencyProperty.Register("TitleOpacity", typeof(double), typeof(MetroFlowItem), new PropertyMetadata(1d));
		#endregion
	}
}
