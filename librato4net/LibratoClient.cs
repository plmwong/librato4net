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
        private readonly ILibratoSettings _settings;

        public LibratoClient(Func<IWebClient> webClientFactory, ILibratoSettings settings)
        {
            _webClientFactory = webClientFactory;
            _settings = settings;
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
                webClient.Credentials = new NetworkCredential(_settings.Username, _settings.ApiKey);

                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                var jsonData = JsonConvert.SerializeObject(payload, LibratoJson.Settings);

                webClient.UploadString(_settings.ApiEndpoint.AbsoluteUri + resource, jsonData);
            }
        }
    }
}