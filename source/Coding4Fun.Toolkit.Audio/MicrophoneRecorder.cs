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

		public MicrophoneRecorder()
		{
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

		public override void Start()
		{
			base.Start();

			_microphone.BufferReady += MicrophoneBufferReady;

			_micBuffer = new byte[_microphone.GetSampleSizeInBytes(_microphone.BufferDuration)];
			_recordStream = new MemoryStream();
			Buffer = null;

			XnaFrameworkDispatcherService.StartService();
			
			_microphone.Start();
		}

    	public override void Stop()
		{
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

    		base.Stop();
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