using System;

namespace Coding4Fun.Toolkit.Audio
{
	public class BufferEventArgs<T> : EventArgs
	{
		public T Buffer { get; set; }
		public Exception Error { get; set; }
	}
}
