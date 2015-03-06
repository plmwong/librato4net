using System;
using System.Diagnostics;

namespace librato4net
{
    public class TimedContext : IDisposable
    {
        private readonly MetricsPublisher _publisher;
        private readonly string _metricName;
        private readonly Stopwatch _stopwatch;

        public TimedContext(MetricsPublisher publisher, string metricName)
        {
            _publisher = publisher;
            _metricName = metricName;
            _stopwatch = new Stopwatch();

            _stopwatch.Start();
        }

        public long ElapsedMilliseconds
        {
            get
            {
                return _stopwatch.ElapsedMilliseconds;
            }
        }

        public void Dispose()
        {
            _stopwatch.Stop();
			_publisher.Measure(_metricName, _stopwatch.ElapsedMilliseconds);
        }
    }
}