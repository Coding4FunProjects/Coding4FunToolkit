using Coding4Fun.Toolkit.Audio;
using Coding4Fun.Toolkit.Test.WinPhone81.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Coding4Fun.Toolkit.Test.WinPhone81.Samples
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Audio : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private const string AudioFileName = "audio.m4a";
        private string _fileName;

        public Audio()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private readonly MicrophoneRecorder _micRecorder = new MicrophoneRecorder();

        private async Task Play(IRandomAccessStream buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            var storageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            if (!string.IsNullOrEmpty(_fileName))
            {
                var oldFile = await storageFolder.GetFileAsync(_fileName);
                await oldFile.DeleteAsync();
            }

            await Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    var storageFile = await storageFolder.CreateFileAsync(AudioFileName, CreationCollisionOption.GenerateUniqueName);

                    _fileName = storageFile.Name;

                    using (var fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await RandomAccessStream.CopyAndCloseAsync(
                            buffer.GetInputStreamAt(0),
                            fileStream.GetOutputStreamAt(0));

                        await buffer.FlushAsync();
                        buffer.Dispose();
                    }

                    var stream = await storageFile.OpenAsync(FileAccessMode.Read);
                    playBack.SetSource(stream, storageFile.FileType);

                    playBack.Play();
                });
        }

        #region testing start / stop

        private void StartRecordingChecked(object sender, RoutedEventArgs e)
        {
            _micRecorder.Start();
        }

        private async void StartRecordingUnchecked(object sender, RoutedEventArgs e)
        {
            _micRecorder.Stop();

            await Play(_micRecorder.Buffer);
        }

        private void StartRecordingWithEventChecked(object sender, RoutedEventArgs e)
        {
            _micRecorder.BufferReady += StartStopBufferReady;
            _micRecorder.Start();
        }

        private void StartRecordingWithEventUnchecked(object sender, RoutedEventArgs e)
        {
            _micRecorder.Stop();
        }

        private async void StartStopBufferReady(object sender, BufferEventArgs<InMemoryRandomAccessStream> e)
        {
            _micRecorder.BufferReady -= StartStopBufferReady;

            await Play(e.Buffer);
        }
        #endregion

        #region testing events and record for timespan

        private void RecordForThreeSecondsClick(object sender, RoutedEventArgs e)
        {
            _micRecorder.BufferReady += StartStopBufferReady;

            _micRecorder.Start(TimeSpan.FromSeconds(3));
        }

        #endregion

        private async void RecordAndAutoTerminateClick(object sender, RoutedEventArgs e)
        {
            _micRecorder.BufferReady += StartStopBufferReady;

            _micRecorder.Start(TimeSpan.FromSeconds(10));

            await ThreadPool.RunAsync(
                async state =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    _micRecorder.Stop();
                });
        }

        #region nav testing

        // testing if nav breaks mic recording
        private void NavTestClick(object sender, RoutedEventArgs e)
        {
            //ThreadPool.QueueUserWorkItem(
            //	state =>
            //		{
            //			Thread.Sleep(500);
            //			Dispatcher.BeginInvoke(() => NavigateTo("/Samples/Audio.xaml"));
            //		});

            //NavigateTo("/MainPage.xaml");
        }

        private void NavigateTo(string page)
        {
            //NavigationService.Navigate(new Uri(page, UriKind.Relative));
        }

        #endregion

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
