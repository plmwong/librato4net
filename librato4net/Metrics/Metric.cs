using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace librato4net.Metrics
{
    public class Metric
    {
        public static Metric CombineAll(params Metric[] metrics)
        {
            var combinedMetric = new Metric
                                            {
                                                Gauges = metrics.Where(m => m.Gauges != null).SelectMany(m => m.Gauges).ToList(),
                                                Counters = metrics.Where(m => m.Counters != null).SelectMany(m => m.Counters).ToList()
                                            };

            if (!combinedMetric.Gauges.Any() && !combinedMetric.Counters.Any())
            {
                return null;
            }

            return combinedMetric;
        }

        [JsonProperty("gauges")]
        public IList<Gauge> Gauges { get; set; }

        [JsonProperty("counters")]
        public IList<Counter> Counters { get; set; }
    }
}

