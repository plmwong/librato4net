using Newtonsoft.Json;

namespace librato4net.Metrics
{
    public class Gauge : Measurement
    {
        [JsonProperty("count")]
        public int? Count { get; set; }

        [JsonProperty("sum")]
        public Number Sum { get; set; }

        [JsonProperty("max")]
        public Number Max { get; set; }

        [JsonProperty("min")]
        public Number Min { get; set; }

        [JsonProperty("sum_squares")]
        public Number SumSquares { get; set; }
    }
}

