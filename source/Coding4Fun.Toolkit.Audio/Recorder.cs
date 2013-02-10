using System;

namespace Coding4Fun.Toolkit.Audio
{
	public abstract class Recorder<T>
	{
		protected bool CatastrophicFailure = false;
		protected Exception FailureException;

		public event EventHandler<BufferEventArgs<T>> BufferReady;

		private bool _shouldCallStopInTimeout;
		private static bool _currentlyProcessing;

		protected Recorder()
		{
			ValidateState();
		}

		public T Buffer { get; set; }

		public virtual int SampleRate { get { return 0; } }

		public virtual void Start()
		{
			ValidateState();

			CatastrophicFailure = false;
			FailureException = null;

			_currentlyProcessing = true;
		}

		public void Start(TimeSpan timeout)
		{
			Start();
			_shouldCallStopInTimeout = true;

			ExecuteStopWithTimeDelay(timeout, _shouldCallStopInTimeout);
		}

		internal virtual void ExecuteStopWithTimeDelay(TimeSpan timeout, bool shouldCallStopInTimeout) { }

		public void Start(int millisecondsTimeout)
		{
			Start(TimeSpan.FromMilliseconds(millisecondsTimeout));
		}

		public virtual void Stop()
		{
			_shouldCallStopInTimeout = false;
			_currentlyProcessing = false;

			if (BufferReady != null)
				BufferReady(this, new BufferEventArgs<T> { Buffer = Buffer, Error = FailureException });
		}

		private static void ValidateState()
		{
			if (_currentlyProcessing)
				throw new InvalidOperationException("cannot excute two records at the same time");
		}
	}
}