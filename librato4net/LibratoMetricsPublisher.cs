using System;
using librato4net.Metrics;
using System.Collections.Generic;

namespace librato4net
{
    public class LibratoMetricsPublisher : MetricsPublisher
    {
		private readonly ILibratoClient _libratoClient;

		public LibratoMetricsPublisher(ILibratoClient libratoClient)
		{
			_libratoClient = libratoClient;
		}

        internal override void Measure(string metricName, object value, string source = null, DateTime? measureTime = null)
        {
			var gaugeMeasurement = new Gauge { Name = metricName, Value = value, Source = source, MeasureTimeValue = measureTime };
			var metric = new Metric { Gauges = new List<Gauge> { gaugeMeasurement } };

			_libratoClient.SendMetric(metric);
        }
    }
}