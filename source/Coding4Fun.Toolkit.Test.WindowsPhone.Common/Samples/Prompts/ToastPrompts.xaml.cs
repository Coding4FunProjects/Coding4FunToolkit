using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Coding4Fun.Toolkit.Controls;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Prompts
{
	public partial class ToastPrompts : PhoneApplicationPage
	{
		private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 240, 248, 255));
		private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 100, 149, 237));

		const string LongText = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";

		public ToastPrompts()
		{
			InitializeComponent();
		}

		#region toast
		#region basic toast
		private void ToastBasicClick(object sender, RoutedEventArgs e)
		{
			var toast = GetBasicToast();

			toast.Show();
		}

		private void ToastWrapBasicClick(object sender, RoutedEventArgs e)
		{
			var toast = GetBasicToast();
			toast.TextWrapping = TextWrapping.Wrap;

			toast.Show();
		}

		private static ToastPrompt GetBasicToast(string title = "Basic")
		{
			return new ToastPrompt
			{
				Title = title,
				Message = LongText,
			};
		}
		#endregion
		#region toast img and no title
		private void ToastWithImgAndNoTitleClick(object sender, RoutedEventArgs e)
		{
			var toast = GetToastWithImgAndNoTitle();

			toast.Show();
		}

		private void ToastWrapWithImgAndNoTitleClick(object sender, RoutedEventArgs e)
		{
			var toast = GetToastWithImgAndNoTitle();
			toast.TextWrapping = TextWrapping.Wrap;

			toast.Show();
		}

		private static ToastPrompt GetToastWithImgAndNoTitle()
		{
			return new ToastPrompt
			{
				Message = LongText,
				ImageSource = new BitmapImage(new Uri("../../media/c4f_26x26.png", UriKind.RelativeOrAbsolute))
			};
		}
		#endregion
		#region toast img and title
		private void ToastWithImgAndTitleClick(object sender, RoutedEventArgs e)
		{
			var toast = GetToastWithImgAndTitle();

			toast.Show();
		}

		private void ToastWrapWithImgAndTitleClick(object sender, RoutedEventArgs e)
		{
			var toast = GetToastWithImgAndTitle();
			toast.TextWrapping = TextWrapping.Wrap;

			toast.Show();
		}

		private static ToastPrompt GetToastWithImgAndTitle()
		{
			return new ToastPrompt
			{
				Title = "With Image",
				TextOrientation = System.Windows.Controls.Orientation.Vertical,
				Message = LongText,
				ImageSource = new BitmapImage(new Uri("../../ApplicationIcon.png", UriKind.RelativeOrAbsolute))
			};
		}
		#endregion
		#region toast with custom everything and event
		private void ToastAdvancedClick(object sender, RoutedEventArgs e)
		{
			var toast = GetAdvancedToast();

			toast.Show();
		}

		private void ToastWrapAdvancedClick(object sender, RoutedEventArgs e)
		{
			var toast = GetAdvancedToast();
			toast.TextWrapping = TextWrapping.Wrap;

			toast.Show();
		}

		private ToastPrompt GetAdvancedToast()
		{
			var toast = new ToastPrompt
			{
				IsAppBarVisible = false,
				Background = _aliceBlueSolidColorBrush,
				Foreground = _cornFlowerBlueSolidColorBrush,
				Title = "Advanced",
				Message = "Custom Fontsize, img, and orientation",
				FontSize = 50,
				TextOrientation = System.Windows.Controls.Orientation.Vertical,
				ImageSource =
					new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute))
			};

			toast.Completed += PopUpPromptStringCompleted;

			return toast;
		}
		#endregion
		#region stress test
		private void ToastSysTrayVisClick(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray();
			GetBasicToast("Test Vis").Show();
		}

		private void ToastSysTrayNotVisClick(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray(false);
			GetBasicToast("Test not Vis").Show();
		}

		private void ToastSysTrayVisWithOpacityClick(object sender, RoutedEventArgs e)
		{
			AdjustSystemTray(true, .8);
			GetBasicToast("Test with Opacity").Show();
		}
		#endregion
		#endregion

		void PopUpPromptStringCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
		{
			resultBlock.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
		}

		private static void AdjustSystemTray(bool isVisible = true, double opacity = 1)
		{
			SystemTray.IsVisible = isVisible;
			SystemTray.Opacity = opacity;
		}
	}
}