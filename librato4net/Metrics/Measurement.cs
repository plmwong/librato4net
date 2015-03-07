using System;
using Newtonsoft.Json;

namespace librato4net.Metrics
{
    public abstract class Measurement
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public Number Value { get; set; }

        [JsonIgnore]
        public DateTime? MeasureTimeValue { get; set; }

        [JsonProperty("measure_time")]
        public long? MeasureTime
        {
            get
            {
                if (MeasureTimeValue.HasValue)
                {
                    return GetUnixTimestamp(MeasureTimeValue.Value);
                }

                return null;
            }
        }

        [JsonProperty("source")]
        public string Source { get; set; }

        private long GetUnixTimestamp(DateTime dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (long)Math.Floor((dateTime - origin).TotalSeconds);
        }
    }
}

