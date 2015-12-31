using System;
using Newtonsoft.Json;

namespace librato4net.Annotations
{
    public class Link
    {
        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}

