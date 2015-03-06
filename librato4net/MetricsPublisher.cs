using System;

namespace librato4net
{
    public abstract class MetricsPublisher
    {
		private static readonly object PublisherLock = new object();
		private static volatile MetricsPublisher _publisher;

		public static void Start(MetricsPublisher metricsPublisher)
		{
			if (_publisher == null)
			{
				lock (PublisherLock)
				{
					if (_publisher == null)
					{
						_publisher = metricsPublisher;
					}
				}
			}
		}

		public static void Start(string source)
		{
			Start(new LibratoMetricsPublisher(new LibratoBufferingClient(new LibratoClient(() => new WebClientAdapter())), source));
		}

        public static MetricsPublisher Current
        {
            get
            {
                return _publisher;
            }
        }

		protected ObservableConcurrentDictionary<string, long> CurrentCounts { get; private set; }

		protected MetricsPublisher() 
		{
			CurrentCounts = new ObservableConcurrentDictionary<string, long>();
			CurrentCounts.CollectionChanged += CountsChanged;
		}

		protected MetricsPublisher(string source) : this()
		{
			Source = source;
		}

		protected abstract void CountsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e);

		protected string Source { get; private set; }

		internal abstract void Measure(string metricName, Number value);

		internal abstract void Increment(string metricName, long @by = 1);

        internal TimedContext Time(string metricName)
        {
			return new TimedContext(Current, metricName);
        }
    }

    public static class MetricsPublisherExtensions
    {
		public static void Measure(this MetricsPublisher publisher, string metricName, Number value)
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
