using System;
using System.IO;

using System.Windows.Media;

namespace Coding4Fun.Toolkit.Audio
{
	public class MemoryStreamAudioSink : AudioSink
	{
		public AudioFormat AudioFormat { get; private set; }
		public MemoryStream AudioData { get; set; }

		protected override void OnCaptureStarted()
		{
			ResetAudioData();
		}

		protected override void OnCaptureStopped() { }

		protected override void OnFormatChange(AudioFormat audioFormat)
		{
			if (AudioFormat == null)
			{
				AudioFormat = audioFormat;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		protected override void OnSamples(long sampleTime, long sampleDuration, byte[] sampleData)
		{
			AudioData.Write(sampleData, 0, sampleData.Length);
		}

		public void ResetAudioData()
		{
			AudioData = new MemoryStream();
		}
	}
}