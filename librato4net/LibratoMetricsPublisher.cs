using System;
using librato4net.Metrics;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using librato4net.Annotations;

namespace librato4net
{
    public class LibratoMetricsPublisher : MetricsPublisher
    {
        private readonly ILibratoClient _libratoClient;

        public LibratoMetricsPublisher(ILibratoClient libratoClient, string source = null)
            : base(source)
        {
            _libratoClient = libratoClient;
        }

        internal override void Annotate(string type, string title, string description, DateTime? startTime, DateTime? endTime)
        {
            var annotation = new Annotation
            {
                Type = type,
                Title = title,
                Description = description,
                StartTime = startTime,
                EndTime = endTime
            };
            
            _libratoClient.SendAnnotation(annotation);
        }

        internal override void Measure(string metricName, Number value)
        {
            var gaugeMeasurement = new Gauge { Name = metricName, Value = value, Source = Source, MeasureTimeValue = DateTime.UtcNow };
            var metric = new Metric { Gauges = new List<Gauge> { gaugeMeasurement } };

            _libratoClient.SendMetric(metric);
        }

        internal override void Increment(string metricName, long @by = 1)
        {
            CurrentCounts.AddOrUpdate(metricName, @by, (id, count) => count + @by);
        }

        protected override void CountsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var changedItems = e.NewItems;

            foreach (var changedItem in changedItems.Cast<KeyValuePair<string, long>>())
            {
                var counterMeasurement = new Counter { Name = changedItem.Key, Value = changedItem.Value, Source = Source, MeasureTimeValue = DateTime.UtcNow };
                var metric = new Metric { Counters = new List<Counter> { counterMeasurement } };

                _libratoClient.SendMetric(metric);
            }
        }
    }
}
