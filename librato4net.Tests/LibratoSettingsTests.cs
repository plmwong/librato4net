using NUnit.Framework;

namespace librato4net.Tests
{
    [TestFixture]
    public class LibratoSettingsTests
    {
        private ILibratoSettings _settings;
        
        [SetUp]
        public void SetUp()
        {
            _settings = new AppSettingsLibratoSettings();
        }
        
        [Test]
        public void can_read_apikey_from_appsettings_configuration()
        {
            Assert.That(_settings.ApiKey, Is.EqualTo("YOUR_APPSETTINGS_API_KEY"));
        }

        [Test]
        public void can_read_username_from_appsettings_configuration()
        {
            Assert.That(_settings.Username, Is.EqualTo("YOUR_APPSETTINGS_USERNAME"));
        }
    }
}

