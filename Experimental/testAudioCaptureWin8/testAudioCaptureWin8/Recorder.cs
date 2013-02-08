using System;
using testAudioCaptureWin8;


#if WINDOWS_STORE
using System.Threading.Tasks;
using Windows.System.Threading;

#elif WINDOWS_PHONE
using System.Threading;

#endif

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

		protected T Buffer { get; set; }

		public virtual int SampleRate { get { return 0; } }

		public virtual async Task<bool> Start()
		{
			ValidateState();

			CatastrophicFailure = false;
			FailureException = null;

			_currentlyProcessing = true;

			return true;
		}

		public async Task<bool> Start(TimeSpan timeout)
		{
			var returnData = await Start();
			_shouldCallStopInTimeout = true;

#if WINDOWS_STORE
			ThreadPool.RunAsync(
					async state =>
					{
						await Task.Delay(timeout);

						if (_shouldCallStopInTimeout)
							await Stop();
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
			return returnData;
		}

		public async Task<bool> Start(int millisecondsTimeout)
		{
			return await Start(TimeSpan.FromMilliseconds(millisecondsTimeout));
		}

		public async virtual Task<bool> Stop()
		{
			_shouldCallStopInTimeout = false;
			_currentlyProcessing = false;

			if (BufferReady != null)
				BufferReady(this, new BufferEventArgs<T> { Buffer = Buffer, Error = FailureException});


			return true;
		}

		private static void ValidateState()
		{
			if (_currentlyProcessing)
				throw new InvalidOperationException("cannot excute two records at the same time");
		}
	}
}