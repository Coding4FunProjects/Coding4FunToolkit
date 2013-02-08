using System.IO;
using System.Windows.Media;

namespace Coding4Fun.Toolkit.Audio
{
	public class MicrophoneRecorder : Recorder<MemoryStream>
    {
		private MemoryStreamAudioSink _audio;
		private CaptureSource _source;

		public MicrophoneRecorder()
		{
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

			_source.Dispatcher.BeginInvoke(() =>
				                               {
					                               _audio.ResetAudioData();
					                               _source.Start();

					                               base.Start();
				                               });
		}

		public override void Stop()
		{
			_source.Dispatcher.BeginInvoke(
				() =>
					{
						_source.Stop();

						Buffer = _audio.AudioData;
						base.Stop();
					});
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