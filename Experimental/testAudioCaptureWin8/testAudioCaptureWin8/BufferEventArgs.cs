using System;

namespace testAudioCaptureWin8
{
	public class BufferEventArgs<T> : EventArgs
	{
		public T Buffer { get; set; }
		public Exception Error { get; set; }
	}
}
