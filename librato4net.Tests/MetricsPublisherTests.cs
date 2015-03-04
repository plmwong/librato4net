using Moq;
using NUnit.Framework;

namespace librato4net.Tests
{
    [TestFixture]
    public class MetricsPublisherTests
    {
        [Test]
        public void when_publisher_has_not_been_initialised_measure_still_executes()
        {
            Assert.DoesNotThrow(() => MetricsPublisherExtensions.Measure(MetricsPublisher.Current, "some.metric.name", It.IsAny<int>()));
        }

        [Test]
        public void when_publisher_has_not_been_initialised_timing_still_executes()
        {
            Assert.DoesNotThrow(() =>
            {
                using (MetricsPublisherExtensions.Time(MetricsPublisher.Current, "some.metric.name"))
                {
                }
            });
        }
    }
}