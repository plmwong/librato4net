using System;
using System.Configuration;

namespace librato4net
{
    public class LibratoSettings : ConfigurationSection, ILibratoSettings
    {
        // ReSharper disable once InconsistentNaming
        private static readonly LibratoSettings settings = ConfigurationManager.GetSection("LibratoSettings") as LibratoSettings ?? new LibratoSettings();

        private const string DefaultApiEndPoint = "https://metrics-api.librato.com/v1/";

        public static LibratoSettings Settings
        {
            get { return settings; }
        }

        [ConfigurationProperty("username", IsRequired = true, DefaultValue = "")]
        [StringValidator]
        public string Username
        {
            get { return (string)this["username"]; }
            set { this["username"] = value; }
        }

        [ConfigurationProperty("apikey", IsRequired = true, DefaultValue = "")]
        [StringValidator]
        public string ApiKey
        {
            get { return (string)this["apikey"]; }
            set { this["apikey"] = value; }
        }

        [ConfigurationProperty("endpoint", IsRequired = false, DefaultValue = DefaultApiEndPoint)]
        public Uri ApiEndpoint
        {
            get { return (Uri)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }

        [ConfigurationProperty("sendInterval", IsRequired = false, DefaultValue = "00:00:05")]
        [PositiveTimeSpanValidator]
        public TimeSpan SendInterval
        {
            get { return (TimeSpan)this["sendInterval"]; }
            set { this["sendInterval"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}