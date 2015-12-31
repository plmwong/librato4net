using Newtonsoft.Json;

namespace librato4net
{
    public static class LibratoJson
    {
        public static JsonSerializerSettings Settings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new[] { new UnixDateTimeConverter() }
                };
            }
        }
    }
}

