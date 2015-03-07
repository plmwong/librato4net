using NUnit.Framework;

namespace librato4net.Tests
{
    [TestFixture]
    public class LibratoSettingsTests
    {
        [Test]
        public void can_read_apikey_from_application_configuration()
        {
            Assert.That(LibratoSettings.Settings.ApiKey, Is.EqualTo("YOUR_API_KEY"));
        }

        [Test]
        public void can_read_username_from_application_configuration()
        {
            Assert.That(LibratoSettings.Settings.Username, Is.EqualTo("YOUR_USERNAME"));
        }
    }
}

