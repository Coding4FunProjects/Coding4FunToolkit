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
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Coding4Fun.Toolkit.Test.WinPhone81.Samples.Prompts
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MessagePrompts : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public MessagePrompts()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
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

        private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 240, 248, 255));
        private readonly SolidColorBrush _naturalBlueSolidColorBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 135, 189));
        private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(200, 100, 149, 237));

        const string LongText = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";

        private MessagePrompt _prompt;

        private void InitializePrompt()
        {
            //var reuseObject = ReuseObject.IsChecked.GetValueOrDefault(false);

            if (_prompt != null)
            {
                _prompt.Completed -= PromptCompleted;
            }

            //if (!reuseObject || _prompt == null)
            {
                _prompt = new MessagePrompt();
            }

            _prompt.Completed += PromptCompleted;
        }

        private void PromptCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            Results.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
        }

        private void MessageClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Basic Message";
            _prompt.Message = LongText;

            _prompt.Show();
        }

        private void MessageAdvancedClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Advanced Message";
            _prompt.Message = "When complete, i'll navigate back";
            _prompt.Overlay = _cornFlowerBlueSolidColorBrush;
            _prompt.IsCancelVisible = true;

            _prompt.Show();
        }

        private void MessageSuperClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Advanced Message";
            _prompt.Background = _naturalBlueSolidColorBrush;
            _prompt.Foreground = _aliceBlueSolidColorBrush;
            _prompt.Overlay = _cornFlowerBlueSolidColorBrush;

            var btnHide = new RoundButton { Label = "Hide" };
            btnHide.Click += (o, args) => _prompt.Hide();

            var btnComplete = new RoundButton { Label = "Complete" };
            btnComplete.Click += (o, args) => _prompt.OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.Ok, Result = "You clicked the Complete Button" });

            _prompt.ActionPopUpButtons.Clear();
            _prompt.ActionPopUpButtons.Add(btnHide);
            _prompt.ActionPopUpButtons.Add(btnComplete);

            _prompt.Show();
        }

        private void MessageCustomClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Custom Body Message";
            _prompt.Background = _naturalBlueSolidColorBrush;
            _prompt.Foreground = _aliceBlueSolidColorBrush;
            _prompt.Overlay = _cornFlowerBlueSolidColorBrush;
            _prompt.IsCancelVisible = true;

            var btn = new Button { Content = "Msg Box" };
            btn.Click += (s, args) => Results.Text = "Hi!";

            _prompt.Body = btn;

            _prompt.Show();
        }

        private void MsgSysTrayVisClick(object sender, RoutedEventArgs e)
        {
            AdjustSystemTray();
            CreateMsgPrompt("Test Vis");
        }

        private void MsgSysTrayNotVisClick(object sender, RoutedEventArgs e)
        {
            AdjustSystemTray(false);
            CreateMsgPrompt("Test not Vis");
        }

        private void MsgSysTrayVisWithOpacityClick(object sender, RoutedEventArgs e)
        {
            AdjustSystemTray(true, .8);
            CreateMsgPrompt("Test with Opacity");
        }

        private static void CreateMsgPrompt(string message)
        {
            var msgPrompt = new MessagePrompt { Title = message, Message = message };
            msgPrompt.Show();
        }

        private async void DingClick(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("CLICK!", "Testing with Click Event").ShowAsync();
        }

        private static async void AdjustSystemTray(bool isVisible = true, double opacity = 1)
        {
            //SystemTray.IsVisible = isVisible;
            //SystemTray.Opacity = opacity;

            var statusBar = StatusBar.GetForCurrentView();
            statusBar.BackgroundOpacity = opacity;
            if (isVisible)
            {
                await statusBar.ShowAsync();
            }
            else
            {
                await statusBar.HideAsync();
            }
        }
    }
}
