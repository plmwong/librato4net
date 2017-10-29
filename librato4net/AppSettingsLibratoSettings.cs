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
            get
            {
                var username = ConfigurationManager.AppSettings["Librato.Username"];

                if (string.IsNullOrEmpty(username))
                {
                    throw new ConfigurationErrorsException("Librato.Username is required and cannot be empty");
                }

                return username;
            }
        }

        public string ApiKey
        {
            get
            {
                var apiKey = ConfigurationManager.AppSettings["Librato.ApiKey"];

                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new ConfigurationErrorsException("Librato.ApiKey is required and cannot be empty");
                }

                return apiKey;
            }
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