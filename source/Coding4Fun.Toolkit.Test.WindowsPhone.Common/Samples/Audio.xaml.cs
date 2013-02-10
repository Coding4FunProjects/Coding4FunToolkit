using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;

using Microsoft.Phone.Controls;

using Coding4Fun.Toolkit.Audio;
using Coding4Fun.Toolkit.Audio.Helpers;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
	public partial class Audio : PhoneApplicationPage
	{
		readonly MicrophoneRecorder _micRecorder = new MicrophoneRecorder();
		int _fileIndex;
		private string _fileName;

		public Audio()
		{
			InitializeComponent();
		}

		private void Play(MemoryStream buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");

			using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (!string.IsNullOrEmpty(_fileName) && storageFolder.FileExists(_fileName))
					storageFolder.DeleteFile(_fileName);
			}

			Dispatcher.BeginInvoke(
				() =>
					{
						using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
						{
							_fileIndex++;
							_fileName = _fileIndex + ".wav";

							var bytes = buffer.GetWavAsByteArray(_micRecorder.SampleRate);

							using (var stream = storageFolder.CreateFile(_fileName))
							{
								stream.Write(bytes, 0, bytes.Length);
							}

							if (!storageFolder.FileExists(_fileName))
								return;

							using (var stream = new IsolatedStorageFileStream(_fileName, FileMode.Open, storageFolder))
							{
								playBack.SetSource(stream);

								playBack.Play();
							}
						}
					});
		}

		#region testing start / stop

		private void StartRecordingChecked(object sender, RoutedEventArgs e)
		{
			_micRecorder.Start();
		}

		private void StartRecordingUnchecked(object sender, RoutedEventArgs e)
		{
			_micRecorder.Stop();

			Play(_micRecorder.Buffer);
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

		private void StartStopBufferReady(object sender, BufferEventArgs<MemoryStream> e)
		{
			Play(e.Buffer);

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

			ThreadPool.QueueUserWorkItem(
				state =>
				{
					Thread.Sleep(TimeSpan.FromSeconds(2));
					Dispatcher.BeginInvoke(_micRecorder.Stop);
				});
		}

		#region nav testing

		// testing if nav breaks mic recording
		private void NavTestClick(object sender, RoutedEventArgs e)
		{
			ThreadPool.QueueUserWorkItem(
				state =>
					{
						Thread.Sleep(500);
						Dispatcher.BeginInvoke(() => NavigateTo("/Samples/Audio.xaml"));
					});

			NavigateTo("/MainPage.xaml");
		}

		private void NavigateTo(string page)
		{
			NavigationService.Navigate(new Uri(page, UriKind.Relative));
		}
		#endregion
	}
}