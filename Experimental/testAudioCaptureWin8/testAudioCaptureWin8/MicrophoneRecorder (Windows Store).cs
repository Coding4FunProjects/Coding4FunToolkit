using System;
using System.Threading.Tasks;

using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;

namespace Coding4Fun.Toolkit.Audio
{
	public class MicrophoneRecorder : Recorder<InMemoryRandomAccessStream>
	{
		private MediaCapture _mediaCap;

		protected void InitMicrophone()
		{
			if (Buffer != null)
				Buffer.Dispose();

			Buffer = new InMemoryRandomAccessStream();

			if (_mediaCap != null)
				return;

			_mediaCap = new MediaCapture();
			_mediaCap.InitializeAsync(new MediaCaptureInitializationSettings {StreamingCaptureMode = StreamingCaptureMode.Audio}).AsTask().Wait();

			_mediaCap.RecordLimitationExceeded += RecordLimitationExceeded;
			_mediaCap.Failed += Failed;
		}

		public override void Start()
		{
			InitMicrophone();
			_mediaCap.StartRecordToStreamAsync(MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto), Buffer);//.AsTask().Wait();

			base.Start();
		}

		public override void Stop()
		{
			 _mediaCap.StopRecordAsync().AsTask().Wait();

			base.Stop();
		}

		private void Failed(MediaCapture currentCaptureObject, MediaCaptureFailedEventArgs currentFailure)
		{
			CatastrophicFailure = true;
			FailureException = new Exception(string.Format("Code: {0}. {1}", currentFailure.Code, currentFailure.Message));

			base.Stop();
		}

		private void RecordLimitationExceeded(MediaCapture currentCaptureObject)
		{
			FailureException = new Exception("Exceeded Record Limitation");

			Stop();
		}
	}
}