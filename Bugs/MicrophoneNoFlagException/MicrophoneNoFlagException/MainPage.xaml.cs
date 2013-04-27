using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Audio;
using Coding4Fun.Toolkit.Audio.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MicrophoneNoFlagException.Resources;

namespace MicrophoneNoFlagException
{
	public partial class MainPage : PhoneApplicationPage
	{

		readonly MicrophoneRecorder _micRecorder = new MicrophoneRecorder();
		int _fileIndex;
		private string _fileName;

		// Constructor
		public MainPage()
		{
			InitializeComponent();

			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
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

		// Sample code for building a localized ApplicationBar
		//private void BuildLocalizedApplicationBar()
		//{
		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.
		//    ApplicationBar = new ApplicationBar();

		//    // Create a new button and set the text value to the localized string from AppResources.
		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
		//    appBarButton.Text = AppResources.AppBarButtonText;
		//    ApplicationBar.Buttons.Add(appBarButton);

		//    // Create a new menu item with the localized string from AppResources.
		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
		//    ApplicationBar.MenuItems.Add(appBarMenuItem);
		//}
	}
}