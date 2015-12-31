using System;
using System.Net;
using librato4net.Annotations;
using librato4net.Metrics;
using Newtonsoft.Json;

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
            Send(metric, "metrics");
        }

        public void SendAnnotation(Annotation annotation)
        {

            Send(annotation, string.Format("annotations/{0}", annotation.Type));
        }

        private void Send<T>(T payload, string resource)
        {
            using (var webClient = _webClientFactory())
            {
                webClient.Credentials = new NetworkCredential(LibratoSettings.Settings.Username, LibratoSettings.Settings.ApiKey);

                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                var jsonConfig = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new[] { new UnixDateTimeConverter() }
                };
                
                var jsonData = JsonConvert.SerializeObject(payload, jsonConfig);

                webClient.UploadString(LibratoSettings.Settings.ApiEndpoint.AbsoluteUri + resource, jsonData);
            }
        }
    }
}