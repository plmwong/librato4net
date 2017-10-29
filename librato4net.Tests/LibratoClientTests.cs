using System.Net;
using librato4net.Annotations;
using librato4net.Metrics;
using Moq;
using NUnit.Framework;

namespace librato4net.Tests
{
    [TestFixture]
    public class LibratoClientTests
    {
        [Test]
        public void use_configured_credentials_when_sending_metrics()
        {
            var mockWebClient = new Mock<IWebClient>();
            mockWebClient.Setup(m => m.Headers).Returns(new WebHeaderCollection());

            var client = new LibratoClient(() => mockWebClient.Object, LibratoSettings.Settings);

            client.SendMetric(It.IsAny<Metric>());

            mockWebClient.VerifySet(m => m.Credentials = It.IsAny<NetworkCredential>(), Times.Once());
        }

        [Test]
        public void use_configured_credentials_when_sending_annotations()
        {
            var mockWebClient = new Mock<IWebClient>();
            mockWebClient.Setup(m => m.Headers).Returns(new WebHeaderCollection());

            var client = new LibratoClient(() => mockWebClient.Object, LibratoSettings.Settings);

            client.SendAnnotation(new Annotation { Type = "some.type" });

            mockWebClient.VerifySet(m => m.Credentials = It.IsAny<NetworkCredential>(), Times.Once());
        }
    }
}