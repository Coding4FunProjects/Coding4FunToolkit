using System;

using Coding4Fun.Toolkit.Audio.Helpers;

#if WINDOWS_STORE
using System.Threading.Tasks;
using Windows.System.Threading;

#elif WINDOWS_PHONE
using System.Threading;

#endif




namespace Coding4Fun.Toolkit.Audio
{
	public abstract class Recorder
	{
		private bool _shouldCallStopInTimeout;
		private static bool _currentlyProcessing;

		public event EventHandler<EventArgs> BufferReady;

		protected Recorder()
		{
			ValidateState();
		}

		public byte[] Buffer
		{
			get;
			internal set;
		}

		public byte[] BufferAsWav { get { return Buffer != null ? Buffer.GetWavAsByteArray(SampleRate) : null; } }

		public virtual int SampleRate { get { return 0; } }

		public virtual void Start()
		{
			ValidateState();
			_currentlyProcessing = true;
		}

		public void Start(TimeSpan timeout)
		{
			Start();
			_shouldCallStopInTimeout = true;

#if WINDOWS_STORE
			ThreadPool.RunAsync(
					state =>
					{
						Task.Delay(timeout);

						if (_shouldCallStopInTimeout)
							Stop();
					});
			
#elif WINDOWS_PHONE
			ThreadPool.QueueUserWorkItem(
				state =>
					{
						Thread.Sleep(timeout);

						if (_shouldCallStopInTimeout)
							Stop();
					});
#endif
		}

		public void Start(int millisecondsTimeout)
		{
			Start(TimeSpan.FromMilliseconds(millisecondsTimeout));
		}

		public virtual void Stop()
		{
			if (BufferReady != null)
				BufferReady(this, new EventArgs());

			_shouldCallStopInTimeout = false;
			_currentlyProcessing = false;
		}

		private static void ValidateState()
		{
			if (_currentlyProcessing)
				throw new InvalidOperationException("cannot excute two records at the same time");
		}
	}
}