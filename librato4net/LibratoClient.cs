using librato4net.Metrics;
using System.Net;
using Newtonsoft.Json;

namespace librato4net
{
	public class LibratoClient : ILibratoClient
    {
		public void SendMetric(Metric metric)
		{
			using (var webClient = new WebClient())
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