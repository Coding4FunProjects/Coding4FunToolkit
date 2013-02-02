using System;
using System.IO;
using System.Threading;

using Microsoft.Xna.Framework.Audio;

namespace Coding4Fun.Phone.Audio
{
    public class MicrophoneRecorder
    {
		private MemoryStream _recordStream;
		private Microphone _microphone;
		private byte[] _micBuffer;

		private bool _shouldCallStopInTimeout;

		public event EventHandler<EventArgs> BufferReady;

		public MicrophoneRecorder()
		{
			InitMicrophone();
		}

		public byte[] Buffer
		{
			get;
			private set;
		}

		public int SampleRate
		{
			get
			{
				InitMicrophone();

				return _microphone.SampleRate;
			}
		}

    	public void Start()
		{
			_microphone.BufferReady += MicrophoneBufferReady;

			_micBuffer = new byte[_microphone.GetSampleSizeInBytes(_microphone.BufferDuration)];
			_recordStream = new MemoryStream();
			Buffer = null;

			XnaFrameworkDispatcherService.StartService();
			
			_microphone.Start();
		}

    	public void Start(TimeSpan timeout)
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

		public void Start(int millisecondsTimeout)
		{
			Start(TimeSpan.FromMilliseconds(millisecondsTimeout));
		}

		public void Stop()
		{
			_shouldCallStopInTimeout = false;
			_microphone.Stop();

			// dump remaining audio into buffer
			// verify cleanup
			ProcessMicrophoneBuffer();

			XnaFrameworkDispatcherService.StopService();
			_microphone.BufferReady -= MicrophoneBufferReady;

			_recordStream.Close();
			
			Buffer = _recordStream.ToArray(); 
			
			_recordStream.Dispose();

			if (BufferReady != null)
				BufferReady(this, new EventArgs());
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