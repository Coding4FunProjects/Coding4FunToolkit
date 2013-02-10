using System;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;

using Coding4Fun.Toolkit.Audio;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class Audio
	{
		private const string AudioFileName = "audio.m4a";
		private string _fileName;

		private readonly MicrophoneRecorder _micRecorder = new MicrophoneRecorder();

		public Audio()
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
				oldFile.DeleteAsync();
			}

			Dispatcher.RunAsync(
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
			await Play(e.Buffer);

			_micRecorder.BufferReady -= StartStopBufferReady;
		}
		#endregion

		#region testing events and record for timespan

		private void RecordForThreeSecondsClick(object sender, RoutedEventArgs e)
		{
			_micRecorder.BufferReady += StartStopBufferReady;

			_micRecorder.Start(TimeSpan.FromSeconds(3));
		}

		#endregion

		private void RecordAndAutoTerminateClick(object sender, RoutedEventArgs e)
		{
			_micRecorder.BufferReady += StartStopBufferReady;

			_micRecorder.Start(TimeSpan.FromSeconds(10));

			ThreadPool.RunAsync(
				state =>
					{
						Task.Delay(TimeSpan.FromSeconds(2)).Wait();
						Dispatcher.RunAsync(CoreDispatcherPriority.Normal, _micRecorder.Stop);
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