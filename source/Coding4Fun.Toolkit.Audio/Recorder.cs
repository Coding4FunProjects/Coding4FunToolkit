using System;

namespace Coding4Fun.Toolkit.Audio
{
	public abstract class Recorder
	{
		public EventHandler<EventArgs> BufferReady;

		public byte[] Buffer
		{
			get;
			internal set;
		}

		public virtual int SampleRate { get { return 0; } }

		public virtual void Start() { }

		public virtual void Start(TimeSpan timeout){ }

		public void Start(int millisecondsTimeout)
		{
			Start(TimeSpan.FromMilliseconds(millisecondsTimeout));
		}

		public virtual void Stop() { }
	}
}