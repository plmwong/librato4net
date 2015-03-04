using System;
using System.Collections.Concurrent;
using librato4net.Metrics;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace librato4net
{
	public class LibratoBufferingClient : ILibratoClient, IDisposable
	{
		private Thread sendingThread;
		private volatile bool shutDownRequested;

		private static readonly TimeSpan StopTimeout = TimeSpan.FromSeconds(5);
		private static readonly TimeSpan BatchingInterval = TimeSpan.FromMilliseconds(250);
		private static readonly TimeSpan SendInterval = TimeSpan.FromSeconds(5);

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
				while (!_buffer.TryPeek(out metric))
				{
					Thread.Sleep(20);

					if (shutDownRequested)
					{
						break;
					}
				}

				CombineBufferedMetricsAndSend();

				Thread.Sleep(SendInterval);
			}

			while (_buffer.TryPeek(out metric))
			{
				CombineBufferedMetricsAndSend();
			}
		}

		private void CombineBufferedMetricsAndSend() 
		{
			var bufferedMetrics = GetBufferedMetrics();
			var combinedMetric = Metric.CombineAll(bufferedMetrics);

			if (combinedMetric != null)
			{
				_libratoClient.SendMetric(combinedMetric);
			}
		}

		private Metric[] GetBufferedMetrics()
		{
			var batchingStopwatch = new Stopwatch();
			batchingStopwatch.Start();

			Metric metric;
			var bufferedMetrics = new List<Metric>();

			while (batchingStopwatch.Elapsed < BatchingInterval) 
			{
				if (_buffer.TryDequeue(out metric)) {
					bufferedMetrics.Add(metric);
				}
			}

			batchingStopwatch.Stop();

			return bufferedMetrics.ToArray();
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

