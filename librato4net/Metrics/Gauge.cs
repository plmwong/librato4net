using Newtonsoft.Json;

namespace librato4net.Metrics
{
	public class Gauge : Measurement
	{
		[JsonProperty("count")]
		public int? Count { get; private set; }

		[JsonProperty("sum")]
		public Number Sum { get; private set; }

		[JsonProperty("max")]
		public Number Max { get; private set; }

		[JsonProperty("min")]
		public Number Min { get; private set; }

		[JsonProperty("sum_squares")]
		public Number SumSquares { get; private set; }
	}
}

