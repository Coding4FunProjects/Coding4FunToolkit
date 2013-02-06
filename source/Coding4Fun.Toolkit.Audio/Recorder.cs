using System;
using System.Threading;

namespace Coding4Fun.Toolkit.Audio
{
	public abstract class Recorder
	{
		private bool _shouldCallStopInTimeout;
		private static bool _currentlyProcessing;

		public EventHandler<EventArgs> BufferReady;

		protected Recorder()
		{
			ValidateState();
		}

		public byte[] Buffer
		{
			get;
			internal set;
		}

		public virtual int SampleRate { get { return 0; } }

		public virtual void Start()
		{
			ValidateState();
			_currentlyProcessing = true;
		}

		public virtual void Start(TimeSpan timeout)
		{
			Start();
			_shouldCallStopInTimeout = true;

			ThreadPool.QueueUserWorkItem(
					state =>
					{
						Thread.Sleep(timeout);

						if (_shouldCallStopInTimeout)
							Stop();
					});
		}

		public void Start(int millisecondsTimeout)
		{
			Start(TimeSpan.FromMilliseconds(millisecondsTimeout));
		}

		public virtual void Stop()
		{
			_shouldCallStopInTimeout = false;

			if (BufferReady != null)
				BufferReady(this, new EventArgs());

			_currentlyProcessing = false;
		}

		private void ValidateState()
		{
			if (_currentlyProcessing)
				throw new InvalidOperationException("cannot excute two records at the same time");
		}
	}
}