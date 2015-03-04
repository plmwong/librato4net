using System.Collections.Generic;
using Newtonsoft.Json;

namespace librato4net.Metrics
{
	public class Metric
	{
		[JsonProperty("gauges")]
		public IList<Gauge> Gauges { get; set; }

		[JsonProperty("counters")]
		public IList<Counter> Counters { get; set; }
	}
}

