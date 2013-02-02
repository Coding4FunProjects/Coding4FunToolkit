using System;
using System.IO;
using System.Threading;

using Microsoft.Xna.Framework.Audio;

namespace Coding4Fun.Toolkit.Audio
{
	public class MicrophoneRecorder : Recorder
    {
		private MemoryStream _recordStream;
		private Microphone _microphone;
		private byte[] _micBuffer;

		private bool _shouldCallStopInTimeout;
		private static bool _currentlyProcessing;

		public MicrophoneRecorder()
		{
			ValidateState();

			InitMicrophone();
		}

		public override int SampleRate
		{
			get
			{
				InitMicrophone();

				return _microphone.SampleRate;
			}
		}

		private void ValidateState()
		{
			if (_currentlyProcessing)
				throw new InvalidOperationException("cannot excute two records at the same time");
		}

		public override void Start()
		{
			ValidateState();

			_currentlyProcessing = true;
			_microphone.BufferReady += MicrophoneBufferReady;

			_micBuffer = new byte[_microphone.GetSampleSizeInBytes(_microphone.BufferDuration)];
			_recordStream = new MemoryStream();
			Buffer = null;

			XnaFrameworkDispatcherService.StartService();
			
			_microphone.Start();
		}

    	public override void Start(TimeSpan timeout)
		{
			Start();
			_shouldCallStopInTimeout = true;

			ThreadPool.QueueUserWorkItem(
				state =>
				{
					Thread.Sleep(timeout);

					if(_shouldCallStopInTimeout)
						Stop();
				});
		}

		public override void Stop()
		{
			_shouldCallStopInTimeout = false;
			
			_microphone.Stop();
			_microphone.BufferReady -= MicrophoneBufferReady;

			XnaFrameworkDispatcherService.StopService();
			XnaFrameworkDispatcherService.UpdateService();

			// dump remaining audio into buffer
			// verify cleanup
			ProcessMicrophoneBuffer();

			_recordStream.Close();
			
			Buffer = _recordStream.ToArray(); 
			
			_recordStream.Dispose();

			if (BufferReady != null)
				BufferReady(this, new EventArgs());

			_currentlyProcessing = false;
		}

		private void MicrophoneBufferReady(object sender, EventArgs e)
		{
			// dump new data into the buffer
			ProcessMicrophoneBuffer();
		}

		private void ProcessMicrophoneBuffer()
		{
			var size = _microphone.GetData(_micBuffer);
			
			if (_recordStream.CanWrite)
				_recordStream.Write(_micBuffer, 0, size);
		}

		private void InitMicrophone()
		{
			if (_microphone == null)
			{
				_microphone = Microphone.Default;

				_microphone.BufferDuration = TimeSpan.FromMilliseconds(100);
			}
		}
    }
}