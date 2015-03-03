using System;

namespace librato4net
{
    public abstract class MetricsPublisher
    {
        private static MetricsPublisher _publisher;

        public static void Initialise()
        {
            _publisher = new LibratoMetricsPublisher();
        }

        public static MetricsPublisher Current
        {
            get
            {
                return _publisher;
            }
        }

        internal abstract void Measure(string metricName, object value);
        internal abstract void Increment(string metricName, int @by);

        internal TimedContext Time(string metricName)
        {
            return new TimedContext(Current, metricName);
        }
    }

    public static class MetricsPublisherExtensions
    {
        public static void Measure(this MetricsPublisher publisher, string metricName, object value)
        {
            if (publisher == null) return;

            publisher.Measure(metricName.ToLowerInvariant(), value);
        }

        public static void Increment(this MetricsPublisher publisher, string metricName, int @by)
        {
            if (publisher == null) return;

            publisher.Increment(metricName.ToLowerInvariant(), @by);
        }

        public static IDisposable Time(this MetricsPublisher publisher, string metricName)
        {
            if (publisher == null)
            {
                return new EmptyDisposable();
            }

            return publisher.Time(metricName.ToLowerInvariant());
        }
    }
}