using System;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace testAudioCaptureWin8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		private bool _isRecording;
		private const string AudioFileName = "audio.m4a";
		private string _fileName;

		private MediaCapture _mediaCap;
		private InMemoryRandomAccessStream _memStream;
		private StorageFile _storageFile;

        public MainPage()
        {
            InitializeComponent();
        }

		private async void PrepClick(object sender, RoutedEventArgs e)
		{
			if (_isRecording)
				return;

			_mediaCap = new MediaCapture();
			await _mediaCap.InitializeAsync(new MediaCaptureInitializationSettings { StreamingCaptureMode = StreamingCaptureMode.Audio });

			_mediaCap.RecordLimitationExceeded += RecordLimitationExceeded;
			_mediaCap.Failed += Failed;
		}

		private async void StartClick(object sender, RoutedEventArgs e)
		{
			if (_isRecording)
				return;

			_fileName = AudioFileName;

			//_storageFile = await KnownFolders.VideosLibrary.CreateFileAsync(_fileName, CreationCollisionOption.GenerateUniqueName);
			_storageFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFileAsync(_fileName, CreationCollisionOption.GenerateUniqueName);
			_memStream = new InMemoryRandomAccessStream();

			var recordProfile = MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto);

			//await _mediaCap.StartRecordToStorageFileAsync(recordProfile, _storageFile);
			await _mediaCap.StartRecordToStreamAsync(recordProfile, _memStream);

			_isRecording = true;
		}

		private async void StopClick(object sender, RoutedEventArgs e)
		{
			if (!_isRecording)
				return;

			await _mediaCap.StopRecordAsync();
			_isRecording = false;
		}
		
		private async void PlayFileClick(object sender, RoutedEventArgs e)
		{
			using (var fileStream = await _storageFile.OpenAsync(FileAccessMode.ReadWrite))
			{
				await RandomAccessStream.CopyAndCloseAsync(_memStream.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
			}

			var stream = await _storageFile.OpenAsync(FileAccessMode.Read);

			playBack.SetSource(stream, _storageFile.FileType);
			playBack.Play();
		}

		public void Failed(MediaCapture currentCaptureObject, MediaCaptureFailedEventArgs currentFailure)
		{
			try
			{
				Dispatcher.RunAsync(
					CoreDispatcherPriority.Normal,
					() =>
					{
						try
						{
							ShowStatusMessage("Fatal error" + currentFailure.Message);
						}
						catch (Exception e)
						{
							ShowExceptionMessage(e);
						}
					});
			}
			catch (Exception e)
			{
				ShowExceptionMessage(e);
			}
		}

		public async void RecordLimitationExceeded(MediaCapture currentCaptureObject)
		{
			try
			{
				if (_isRecording)
				{
					await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
					{
						try
						{
							await _mediaCap.StopRecordAsync();
							_isRecording = false;

							ShowStatusMessage("Stopping Record on exceeding max record duration");
						}
						catch (Exception e)
						{
							ShowExceptionMessage(e);
						}

					});
				}
			}
			catch (Exception e)
			{
				ShowExceptionMessage(e);
			}
		}

		private void ShowStatusMessage(String text)
		{
			var msg = new MessageDialog(text);
			msg.ShowAsync();
		}

		private void ShowExceptionMessage(Exception ex)
		{
			var msg = new MessageDialog(ex.Message);
			msg.ShowAsync();
		}
    }
}
