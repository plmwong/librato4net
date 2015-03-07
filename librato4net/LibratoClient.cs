using librato4net.Metrics;
using Newtonsoft.Json;
using System;
using System.Net;

namespace librato4net
{
    public class LibratoClient : ILibratoClient
    {
        private readonly Func<IWebClient> _webClientFactory;

        public LibratoClient(Func<IWebClient> webClientFactory)
        {
            _webClientFactory = webClientFactory;
        }

        public void SendMetric(Metric metric)
        {
            using (var webClient = _webClientFactory())
            {
                webClient.Credentials = new NetworkCredential(LibratoSettings.Settings.Username, LibratoSettings.Settings.ApiKey);

                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                var jsonConfig = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var jsonData = JsonConvert.SerializeObject(metric, jsonConfig);

                webClient.UploadString(LibratoSettings.Settings.ApiEndpoint.AbsoluteUri, jsonData);
            }
        }
    }
}