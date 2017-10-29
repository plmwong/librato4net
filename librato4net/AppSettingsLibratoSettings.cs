using System;
using System.Configuration;

namespace librato4net
{
    public class AppSettingsLibratoSettings : ILibratoSettings
    {
        // ReSharper disable once InconsistentNaming
        private static readonly AppSettingsLibratoSettings settings = new AppSettingsLibratoSettings();

        private const string DefaultApiEndPoint = "https://metrics-api.librato.com/v1/";

        public static AppSettingsLibratoSettings Settings
        {
            get { return settings; }
        }

        public string Username
        {
            get { return ConfigurationManager.AppSettings["Librato.Username"]; }
        }

        public string ApiKey
        {
            get { return ConfigurationManager.AppSettings["Librato.ApiKey"]; }
        }

        public Uri ApiEndpoint
        {
            get { return new Uri(ConfigurationManager.AppSettings["Librato.ApiEndpoint"] ?? DefaultApiEndPoint); }
        }

        public TimeSpan SendInterval
        {
            get { return TimeSpan.Parse(ConfigurationManager.AppSettings["Librato.SendInterval"] ?? "00:00:05"); }
        }
    }
}