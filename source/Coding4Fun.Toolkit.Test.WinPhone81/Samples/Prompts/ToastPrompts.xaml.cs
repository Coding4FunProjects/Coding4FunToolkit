using Coding4Fun.Toolkit.Controls;
using Coding4Fun.Toolkit.Test.WinPhone81.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Coding4Fun.Toolkit.Test.WinPhone81.Samples.Prompts
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ToastPrompts : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 240, 248, 255));
        private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(200, 100, 149, 237));

        const string LongText = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";

        private ToastPrompt _prompt;

        public ToastPrompts()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private void InitializePrompt()
        {
            //var reuseObject = ReuseObject.IsChecked.GetValueOrDefault(false);

            if (_prompt != null)
            {
                _prompt.Completed -= PromptCompleted;
            }

            //if (!reuseObject || _prompt == null)
            {
                _prompt = new ToastPrompt();
            }

            // this is me manually resetting stuff due to the reusability test
            // you don't need to do this.
            // fontsize, foreground, background won't manually be reset

            //_prompt.TextWrapping = TextWrapping.NoWrap;
            //_prompt.ImageSource = null;
            //_prompt.ImageHeight = double.NaN;
            //_prompt.ImageWidth = double.NaN;
            //_prompt.Stretch = Stretch.None;
            //_prompt.IsAppBarVisible = false;
            //_prompt.TextOrientation = System.Windows.Controls.Orientation.Horizontal;

            //_prompt.Message = string.Empty;
            //_prompt.Title = string.Empty;

            _prompt.Completed += PromptCompleted;
        }

        #region toast
        #region basic toast
        private void ToastBasicClick(object sender, RoutedEventArgs e)
        {
            InitializeBasicToast();

            _prompt.Show();
        }

        private void ToastWrapBasicClick(object sender, RoutedEventArgs e)
        {
            InitializeBasicToast(wrap: TextWrapping.Wrap);

            _prompt.Show();
        }

        private void InitializeBasicToast(string title = "Basic", TextWrapping wrap = default(TextWrapping))
        {
            InitializePrompt();

            _prompt.Title = title;
            _prompt.Message = LongText;
            _prompt.TextWrapping = wrap;
        }
        #endregion
        #region toast img and no title
        private void ToastWithImgAndNoTitleClick(object sender, RoutedEventArgs e)
        {
            InitializeToastWithImgAndNoTitle();

            _prompt.Show();
        }

        private void ToastWrapWithImgAndNoTitleClick(object sender, RoutedEventArgs e)
        {
            InitializeToastWithImgAndNoTitle(TextWrapping.Wrap);

            _prompt.Show();
        }

        private void InitializeToastWithImgAndNoTitle(TextWrapping wrap = default(TextWrapping))
        {
            InitializePrompt();

            _prompt.Message = LongText;
            _prompt.ImageSource = new BitmapImage(new Uri("ms-appx:/Media/c4f_26x26.png"));
            _prompt.TextWrapping = wrap;
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
                TextOrientation = Orientation.Vertical,
                Message = LongText,
                ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/Square71x71Logo.scale-240.png"))
            };
        }
        #endregion
        #region toast with custom everything and event
        private void ToastAdvancedClick(object sender, RoutedEventArgs e)
        {
            InitializeAdvancedToast();

            _prompt.Show();
        }

        private void ToastWrapAdvancedClick(object sender, RoutedEventArgs e)
        {
            InitializeAdvancedToast(TextWrapping.Wrap);

            _prompt.Show();
        }

        private void InitializeAdvancedToast(TextWrapping wrap = default(TextWrapping))
        {
            InitializePrompt();

            _prompt.IsAppBarVisible = false;
            _prompt.Title = "Advanced";
            _prompt.Message = "Custom Fontsize, img, and orientation";
            _prompt.ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/StoreLogo.scale-240.png"));
            _prompt.FontSize = 50;
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Background = _aliceBlueSolidColorBrush;
            _prompt.Foreground = _cornFlowerBlueSolidColorBrush;

            _prompt.TextWrapping = wrap;
        }
        #endregion
        #region stress test
        private void ToastSysTrayVisClick(object sender, RoutedEventArgs e)
        {
            AdjustSystemTray();
            InitializeBasicToast("Test Vis");

            _prompt.Show();
        }

        private void ToastSysTrayNotVisClick(object sender, RoutedEventArgs e)
        {
            AdjustSystemTray(false);
            InitializeBasicToast("Test not Vis");

            _prompt.Show();
        }

        private void ToastSysTrayVisWithOpacityClick(object sender, RoutedEventArgs e)
        {
            AdjustSystemTray(true, .8);
            InitializeBasicToast("Test with Opacity");

            _prompt.Show();
        }
        #endregion

        #region large image
        private void LargeImageClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "With Image";
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Message = LongText;
            _prompt.ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/Logo.scale-240.png"));

            _prompt.Show();
        }

        private void LargeImageWidthHeightClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Width + Height";
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Message = LongText;
            _prompt.ImageHeight = 50;
            _prompt.ImageWidth = 100;
            _prompt.ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/Logo.scale-240.png"));

            _prompt.Show();
        }

        private void LargeImageStretchClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Stretch";
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Message = LongText;
            _prompt.ImageHeight = 100;
            _prompt.Stretch = Stretch.Fill;
            _prompt.ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/Logo.scale-240.png"));

            _prompt.Show();
        }

        private void LargeImageStretchWidthHeightClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Stretch + Width + Height";
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Message = LongText;
            _prompt.ImageHeight = 50;
            _prompt.ImageWidth = 100;
            _prompt.Stretch = Stretch.Uniform;
            _prompt.ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/Logo.scale-240.png"));

            _prompt.Show();
        }
        #endregion
        #endregion

        void PromptCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            Results.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
        }

        private static async void AdjustSystemTray(bool isVisible = true, double opacity = 1)
        {
            var statusBar = StatusBar.GetForCurrentView();
            statusBar.BackgroundOpacity = opacity;

            if(isVisible)
            {
                await statusBar.ShowAsync();
            }
            else
            {
                await statusBar.HideAsync();
            }
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
