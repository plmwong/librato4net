using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using librato4net.Annotations;
using librato4net.Metrics;

namespace librato4net
{
    public class LibratoBufferingClient : ILibratoClient, IDisposable
    {
        private Thread _sendingThread;
        private volatile bool _shutDownRequested;

        private static readonly TimeSpan StopTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan BatchingTimeout = TimeSpan.FromSeconds(1);

        private readonly ConcurrentQueue<Metric> _buffer;
        private readonly ILibratoClient _libratoClient;

        private bool _disposed;

        public LibratoBufferingClient(ILibratoClient libratoClient)
        {
            _buffer = new ConcurrentQueue<Metric>();
            _libratoClient = libratoClient;

            StartSending();
        }

        public void SendMetric(Metric metric)
        {
            if (!_shutDownRequested && metric != null)
            {
                _buffer.Enqueue(metric);
            }
        }

        public void SendAnnotation(Annotation annotation)
        {
            if (!_shutDownRequested && annotation != null)
            {
                _libratoClient.SendAnnotation(annotation);
            }
        }

        private void StartSending()
        {
            if (_shutDownRequested)
            {
                return;
            }

            _sendingThread =
                new Thread(SendingThreadExecute)
                    {
                        Name = String.Format("Librato Metric Forwarding Thread"),
                        IsBackground = false,
                    };

            _sendingThread.Start();
        }

        private void StopSending()
        {
            _shutDownRequested = true;

            var hasFinishedFlushingBuffer = _sendingThread.Join(StopTimeout);

            if (!hasFinishedFlushingBuffer)
            {
                _sendingThread.Abort();
            }
        }

        private void SendingThreadExecute()
        {
            while (!_shutDownRequested)
            {
                try
                {
                    SendMetricsFromQueue();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Exception occurred while sending metrics: " + ex);
                }
            }
        }

        private void SendMetricsFromQueue()
        {
            Metric metric;

            while (!_shutDownRequested)
            {
                while (!_buffer.TryPeek(out metric))
                {
                    Thread.Sleep(25);

                    if (_shutDownRequested)
                    {
                        break;
                    }
                }

                CombineBufferedMetricsAndSend();

                Thread.Sleep(LibratoSettings.Settings.SendInterval);
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

            var bufferedMetrics = new List<Metric>();

            while (batchingStopwatch.Elapsed < BatchingTimeout)
            {
                Metric metric;
                if (_buffer.TryDequeue(out metric))
                {
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
            if (_disposed)
            {
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

