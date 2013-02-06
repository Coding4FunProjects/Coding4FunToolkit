using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using Coding4Fun.Toolkit.Audio;
using Coding4Fun.Toolkit.Audio.Helpers;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
	public partial class Audio : PhoneApplicationPage
	{
		MicrophoneRecorder _micRecorder = new MicrophoneRecorder();
		int _fileIndex;
		string _currentFileName;

		public Audio()
		{
			InitializeComponent();
		}

		#region testing start / stop
		private void StartRecordingManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			_micRecorder.Start();
		}

		private void StopRecordingManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			_micRecorder.Stop();

			SaveAndPlay();
		}
		#endregion
		#region testing events and record for timespan
		private void RecordForThreeSecondsClick(object sender, RoutedEventArgs e)
		{
			_micRecorder = new MicrophoneRecorder();
			_micRecorder.BufferReady += StartStopBufferReady;
			_micRecorder.Start(TimeSpan.FromSeconds(3));
		}
		private void StartRecordingManipulationWithEventStarted(object sender, ManipulationStartedEventArgs e)
		{
			_micRecorder.BufferReady += StartStopBufferReady;
			_micRecorder.Start();
		}

		private void StopRecordingManipulationWithEventCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			_micRecorder.Stop();
		}

		private void StartStopBufferReady(object sender, EventArgs e)
		{
			_micRecorder.BufferReady -= StartStopBufferReady;

			SaveAndPlay();
		}
		#endregion

		private void RecordForThreeSecondsAndTerminateClick(object sender, RoutedEventArgs e)
		{
			_micRecorder = new MicrophoneRecorder();
			
			_micRecorder.Start(TimeSpan.FromSeconds(3));

			ThreadPool.QueueUserWorkItem(
				state =>
					{
						Thread.Sleep(TimeSpan.FromSeconds(1));
						_micRecorder.Stop();

						SaveAndPlay();
					});
		}

		private void SaveAndPlay()
		{
			WriteFile(_micRecorder.Buffer.GetWavAsByteArray(_micRecorder.SampleRate));

			PlayFile();
		}

		#region nav testing

		// testing if nav breaks mic recording
		private void NavTestClick(object sender, RoutedEventArgs e)
		{
			ThreadPool.QueueUserWorkItem(state =>
											{
												Thread.Sleep(500);
												Dispatcher.BeginInvoke(() => NavigateTo("/Samples/Audio.xaml"));
											}
			);

			NavigateTo("/MainPage.xaml");
		}

		private void NavigateTo(string page)
		{
			NavigationService.Navigate(new Uri(page, UriKind.Relative));
		}
		#endregion

		#region helper methods for reading/writing/deleting files into file storage for playback
		private void PlayFile()
		{
			Dispatcher.BeginInvoke(() =>
			{
				using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
				{

					using (var stream = new IsolatedStorageFileStream(_currentFileName, FileMode.Open, storageFile))
					{
						playBack.SetSource(stream);
						DeleteOldFile();

						playBack.Play();

					}

				}
			});
		}

		private void WriteFile(byte[] bytes)
		{
			using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
			{
				_fileIndex++;
				_currentFileName = _fileIndex + ".wav";

				using (var stream = storageFile.CreateFile(_currentFileName))
				{
					stream.Write(bytes, 0, bytes.Length);
				}
			}
		}

		private void DeleteOldFile()
		{
			using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
			{
				var currentFileName = (_fileIndex - 1) + ".wav";

				if (storageFile.FileExists(currentFileName))
					storageFile.DeleteFile(currentFileName);
			}
		}
		#endregion

	}
}