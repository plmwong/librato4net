using librato4net.Metrics;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace librato4net.Tests
{
	[TestFixture]
	public class LibratoMetricsPublisherTests
	{
		[Test]
		public void when_measure_is_called_then_a_metric_is_sent() 
		{
			var libratoClient = new Mock<ILibratoClient>();
			var publisher = new LibratoMetricsPublisher(libratoClient.Object);

			publisher.Measure("some.metric.name", It.IsAny<int>());

			libratoClient.Verify(m => m.SendMetric(It.IsAny<Metric>()), Times.Once);
		}

		[Test]
		public void when_increment_is_called_then_a_metric_is_sent() 
		{
			var libratoClient = new Mock<ILibratoClient>();
			var publisher = new LibratoMetricsPublisher(libratoClient.Object);

			publisher.Increment("some.metric.name");

			libratoClient.Verify(m => m.SendMetric(It.IsAny<Metric>()), Times.Once);
		}
	}
}

