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

        internal override void Measure(string metricName, object value)
        {
			var gaugeMeasurement = new Gauge { Name = metricName, Value = value };
			var metric = new Metric { Gauges = new List<Gauge> { gaugeMeasurement } };

			_libratoClient.SendMetric(metric);
        }
    }
}