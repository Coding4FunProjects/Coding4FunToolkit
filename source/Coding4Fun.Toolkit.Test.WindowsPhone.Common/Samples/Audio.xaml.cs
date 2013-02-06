using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using Coding4Fun.Toolkit.Audio;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
	public partial class Audio : PhoneApplicationPage
	{
		readonly MicrophoneRecorder _micRecorder = new MicrophoneRecorder();
		int _fileIndex;
		string _currentFileName;

		public Audio()
		{
			InitializeComponent();

			_micRecorder.BufferReady += StartStopBufferReady;
		}

		private void StartStopBufferReady(object sender, EventArgs e)
		{
			SaveAndPlay();
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
			_micRecorder.Start(TimeSpan.FromSeconds(3));
		}
		#endregion

		private void RecordForThreeSecondsAndTerminateClick(object sender, RoutedEventArgs e)
		{
			_micRecorder.Start(TimeSpan.FromSeconds(3));

			ThreadPool.QueueUserWorkItem(
				state =>
					{
						
						Thread.Sleep(TimeSpan.FromSeconds(2));
						Dispatcher.BeginInvoke(() =>
							                       {
								                       _micRecorder.Stop();
								                       SaveAndPlay();
							                       });
					});
		}

		private void SaveAndPlay()
		{
			WriteFile(_micRecorder.BufferAsWav);

			PlayFile();
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

		#region helper methods for reading/writing/deleting files into file storage for playback

		private void PlayFile()
		{
			DeleteOldFile();

			using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (!storageFile.FileExists(_currentFileName))
					return;

				using (var stream = new IsolatedStorageFileStream(_currentFileName, FileMode.Open, storageFile))
				{
					playBack.SetSource(stream);

					playBack.Play();
				}
			}
		}

		private void WriteFile(byte[] bytes)
		{
			_fileIndex++;
			_currentFileName = _fileIndex + ".wav";

			Debug.WriteLine("Attempting to WriteFile");
			if (bytes == null)
				return;
			
			using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
			{
				using (var stream = storageFile.CreateFile(_currentFileName))
				{
					stream.Write(bytes, 0, bytes.Length);
				}
			}

			Debug.WriteLine("WriteFile: " + _currentFileName);
			Debug.WriteLine("Length: " + bytes.Length);
		}

		private void DeleteOldFile()
		{
			using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
			{
				var targetFileName = (_fileIndex - 1) + ".wav";

				if (storageFile.FileExists(targetFileName))
					storageFile.DeleteFile(targetFileName);
			}
		}
		#endregion

	}
}