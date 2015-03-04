using System;
using System.Diagnostics;

namespace librato4net
{
    public class TimedContext : IDisposable
    {
        private readonly MetricsPublisher _publisher;
        private readonly string _metricName;
        private readonly string _source;
        private readonly DateTime? _measureTime;
        private readonly Stopwatch _stopwatch;

        public TimedContext(MetricsPublisher publisher, string metricName, string source = null, DateTime? measureTime = null)
        {
            _publisher = publisher;
            _metricName = metricName;
            _source = source;
            _measureTime = measureTime;
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
            _publisher.Measure(_metricName, _stopwatch.ElapsedMilliseconds, _source, _measureTime);
        }
    }
}