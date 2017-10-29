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

        public static void Start(string source = null, SettingsSource settingsSource = SettingsSource.Default)
        {
            ILibratoSettings settings;
            
            if (settingsSource == SettingsSource.AppSettings)
            {
                settings = new AppSettingsLibratoSettings();
            } 
            else 
            {
                settings = LibratoSettings.Settings;
            }

            Start(new LibratoMetricsPublisher(new LibratoBufferingClient(new LibratoClient(() => new WebClientAdapter(), settings), settings), source));
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

        protected MetricsPublisher(string source)
            : this()
        {
            Source = source;
        }

        protected abstract void CountsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e);

        protected string Source { get; private set; }

        internal abstract void Annotate(string type, string title, string description, DateTime? startTime, DateTime? endTime);

        internal abstract void Measure(string metricName, Number value);

        internal abstract void Increment(string metricName, long @by = 1);

        internal TimedContext Time(string metricName)
        {
            return new TimedContext(this, metricName);
        }
    }

    public static class MetricsPublisherExtensions
    {
        public static void Annotate(this MetricsPublisher publisher, string type, string title, string description = null, DateTime? startTime = null, DateTime? endTime = null)
        {
            if (publisher == null) return;

            publisher.Annotate(type, title, description, startTime, endTime);
        }
        
        public static void Measure(this MetricsPublisher publisher, string metricName, Number value)
        {
            if (publisher == null) return;

            publisher.Measure(metricName.ToLowerInvariant(), value);
        }

        public static void Increment(this MetricsPublisher publisher, string metricName, long @by = 1)
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
