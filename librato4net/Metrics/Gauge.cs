using Newtonsoft.Json;

namespace librato4net.Metrics
{
	public class Gauge : Measurement
	{
		[JsonProperty("count")]
		public int? Count { get; private set; }

		[JsonProperty("sum")]
		public object Sum { get; private set; }

		[JsonProperty("max")]
		public object Max { get; private set; }

		[JsonProperty("min")]
		public object Min { get; private set; }

		[JsonProperty("sum_squares")]
		public object SumSquares { get; private set; }
	}
}

