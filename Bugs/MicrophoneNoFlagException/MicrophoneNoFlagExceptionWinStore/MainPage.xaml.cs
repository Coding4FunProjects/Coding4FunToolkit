using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Coding4Fun.Toolkit.Audio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MicrophoneNoFlagExceptionWinStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string AudioFileName = "audio.m4a";
		private string _fileName;

		private readonly MicrophoneRecorder _micRecorder = new MicrophoneRecorder();

		public MainPage()
		{
			InitializeComponent();
		}

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
    }
}
