using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Audio.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using testAudioCaptureWp8.Resources;

namespace testAudioCaptureWp8
{
	public partial class MainPage : PhoneApplicationPage
	{
		private MemoryStreamAudioSink _audio;
		private CaptureSource _source;
		// Constructor
		public MainPage()
		{
			InitializeComponent();

			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
		}

		private void StartClick(object sender, RoutedEventArgs e)
		{
			_source = new CaptureSource()
			{
				AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice(),
				VideoCaptureDevice = null,
			};   

			_audio = new MemoryStreamAudioSink {CaptureSource = _source};

			_audio.CaptureSource.Start();
		}

		private void StopClick(object sender, RoutedEventArgs e)
		{
			_audio.CaptureSource.Stop();

			SaveAndPlay();
		}

		private void PlayClick(object sender, RoutedEventArgs e)
		{
			audioPlayer.Position = TimeSpan.Zero;
			audioPlayer.Play();
		}


		#region helper methods for reading/writing/deleting files into file storage for playback
		private void SaveAndPlay()
		{
			using (var tempBuffer = new MemoryStream())
			{
				Wav.WriteHeader(tempBuffer, _audio.AudioFormat.SamplesPerSecond);
				Wav.SeekPastHeader(tempBuffer);

				_audio.AudioData.Position = 0;
				_audio.AudioData.CopyTo(tempBuffer);
				
				Wav.UpdateHeader(tempBuffer);

				WriteFile(tempBuffer.ToArray());
			}

			PlayFile();
		}

		int _fileIndex;
		string _currentFileName;
		private void PlayFile()
		{
			Dispatcher.BeginInvoke(() =>
			{
				using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
				{

					using (var stream = new IsolatedStorageFileStream(_currentFileName, FileMode.Open, storageFile))
					{
						audioPlayer.SetSource(stream);
						DeleteOldFile();

						audioPlayer.Play();

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

		/// <summary>   
		/// This class is going to eat a tonne of memory...   
		/// </summary>   
		public class MemoryStreamAudioSink : AudioSink
		{
			protected override void OnCaptureStarted()
			{
				stream = new MemoryStream();
			}
			protected override void OnCaptureStopped()
			{
			}
			public AudioFormat AudioFormat
			{
				get
				{
					return (audioFormat);
				}
			}
			public MemoryStream AudioData
			{
				get
				{
					return (stream);
				}
			}
			protected override void OnFormatChange(AudioFormat audioFormat)
			{
				if (this.audioFormat == null)
				{
					this.audioFormat = audioFormat;
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
			protected override void OnSamples(long sampleTime, long sampleDuration, byte[] sampleData)
			{
				stream.Write(sampleData, 0, sampleData.Length);
			}
			MemoryStream stream;
			AudioFormat audioFormat;
		}

		

	}
}