using System;
using System.Collections.Concurrent;
using librato4net.Metrics;
using System.Threading;

namespace librato4net
{
	public class LibratoBufferingClient : ILibratoClient, IDisposable
	{
		private Thread sendingThread;
		private volatile bool shutDownRequested;

		private static readonly TimeSpan StopTimeout = TimeSpan.FromSeconds(5);

		private readonly ConcurrentQueue<Metric> _buffer;
		private readonly ILibratoClient _libratoClient;

		bool _disposed;

		public LibratoBufferingClient(ILibratoClient libratoClient)
		{
			_buffer = new ConcurrentQueue<Metric>();
			_libratoClient = libratoClient;

			StartSending();
		}

		public void SendMetric(Metric metric)
		{
			if (!shutDownRequested && metric != null)
			{
				_buffer.Enqueue(metric);
			}
		}

		private void StartSending()
		{
			if (shutDownRequested)
			{
				return;
			}

			sendingThread =
				new Thread(SendingThreadExecute)
					{
						Name = String.Format("Librato Metric Forwarding Thread"),
						IsBackground = false,
					};

			sendingThread.Start();
		}

		private void StopSending()
		{
			shutDownRequested = true;

			var hasFinishedFlushingBuffer = sendingThread.Join(StopTimeout);

			if (!hasFinishedFlushingBuffer)
			{
				sendingThread.Abort();
			}
		}

		private void SendingThreadExecute()
		{
			while (!shutDownRequested)
			{
				try
				{
					SendMetricsFromQueue();
				}
				// Analysis disable once EmptyGeneralCatchClause
				catch (Exception)
				{
				}
			}
		}

		private void SendMetricsFromQueue()
		{
			Metric metric;

			while (!shutDownRequested)
			{
				while (!_buffer.TryDequeue(out metric))
				{
					Thread.Sleep(20);

					if (shutDownRequested)
					{
						break;
					}
				}

				if (metric != null)
				{
					_libratoClient.SendMetric(metric);
				}
			}

			while (_buffer.TryDequeue(out metric))
			{
				if (metric != null)
				{
					_libratoClient.SendMetric(metric);
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~LibratoBufferingClient()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) {
				return;
			}

			if (disposing)
			{
				StopSending();
			}

			_disposed = true;
		}
	}
}

