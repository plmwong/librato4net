using System;

namespace librato4net
{
    public abstract class MetricsPublisher
    {
		private static object _publisherLock = new object();
		private static MetricsPublisher _publisher;

		public static void Start(MetricsPublisher metricsPublisher)
		{
			if (_publisher == null)
			{
				lock (_publisherLock)
				{
					if (_publisher == null)
					{
						_publisher = metricsPublisher;
					}
				}
			}
		}

		public static void Start()
		{
			Start(new LibratoMetricsPublisher(new LibratoBufferingClient(new LibratoClient())));
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