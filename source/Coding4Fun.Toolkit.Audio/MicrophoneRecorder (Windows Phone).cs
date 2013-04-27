using System;
using System.IO;
using System.Threading;
using System.Windows.Media;

using Coding4Fun.Toolkit.Audio.Helpers;

namespace Coding4Fun.Toolkit.Audio
{
	public class MicrophoneRecorder : Recorder<MemoryStream>
    {
		private MemoryStreamAudioSink _audio;
		private CaptureSource _source;
		private DateTime _startTime;
		private readonly TimeSpan _minRunTime = TimeSpan.FromMilliseconds(750);

		public MicrophoneRecorder()
		{
			if (!CapabilityHelper.IsMicrophoneCapability)
			{
				throw new UnauthorizedAccessException("Add the Microphone capability in the application manifest");
			}

			InitMicrophone();
		}

		public override int SampleRate
		{
			get
			{
				InitMicrophone();

				return _audio.AudioFormat.SamplesPerSecond;
			}
		}

		public override void Start()
		{
			InitMicrophone();

			_source.Dispatcher.BeginInvoke(
				() =>
					{
						_audio.ResetAudioData();
						_source.Start();
						_startTime = DateTime.Now;

						base.Start();
					}
				);
		}

		public override void Stop()
		{
			ShouldCallStopInTimeout = false;

			while (_audio.AudioFormat == null)
			{
				Thread.Sleep(1);

				if ((DateTime.Now - _startTime) > _minRunTime)
				{
					break;
				}
			}

			_source.Stop();

			Buffer = _audio.AudioData;
	
			base.Stop();
		}

		internal override void ExecuteStopWithTimeDelay(TimeSpan timeout)
		{
			if (timeout < _minRunTime)
				timeout = _minRunTime;

			ThreadPool.QueueUserWorkItem(
				state =>
				{
					Thread.Sleep(timeout);

					if (ShouldCallStopInTimeout)
						_source.Dispatcher.BeginInvoke(Stop);
				});
			
			base.ExecuteStopWithTimeDelay(timeout);
		}

		private void InitMicrophone()
		{
			if (_audio != null)
				return;

			_source = new CaptureSource
				          {
					          AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice(),
					          VideoCaptureDevice = null,
				          };
			
			_audio = new MemoryStreamAudioSink {CaptureSource = _source};
		}
    }
}