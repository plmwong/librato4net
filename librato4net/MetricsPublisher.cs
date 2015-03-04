using System;

namespace librato4net
{
    public abstract class MetricsPublisher
    {
        private static MetricsPublisher _publisher;

		static MetricsPublisher()
        {
			var client = new LibratoBufferingClient(new LibratoClient());
			_publisher = new LibratoMetricsPublisher(client);
        }

        public static MetricsPublisher Current
        {
            get
            {
                return _publisher;
            }
        }

        internal abstract void Measure(string metricName, object value);

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