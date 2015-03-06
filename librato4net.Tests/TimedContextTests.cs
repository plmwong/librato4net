using Moq;
using NUnit.Framework;

namespace librato4net.Tests
{
    [TestFixture]
    public class TimedContextTests
    {
        [Test]
        public void when_timed_context_ends_then_a_measurement_of_the_time_is_published()
        {
            var mockPublisher = new Mock<MetricsPublisher>();

            using (new TimedContext(mockPublisher.Object, "some.metric.name"))
            {
            }

            mockPublisher.Verify(m => m.Measure("some.metric.name", It.IsAny<Number>()));
        }
    }
}
