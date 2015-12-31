using System;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace librato4net.Tests
{
    [TestFixture]
    public class UnixDateTimeConverterTests
    {
        [Test]
        public void epoch_date_is_serialised_as_0()
        {
            var mockJsonWriter = new Mock<JsonWriter>();
            var converter = new UnixDateTimeConverter();

            converter.WriteJson(mockJsonWriter.Object, new DateTime(1970, 1, 1), null);

            mockJsonWriter.Verify(m => m.WriteValue(0L), Times.Once);
        }

        [TestCase("2015-07-03T00:00:00", 1435881600L)]
        [TestCase("2015-12-30T21:00:51", 1451509251L)]
        public void dates_are_serialised_as_unix_time(string timestamp, long unixTime)
        {
            var mockJsonWriter = new Mock<JsonWriter>();
            var converter = new UnixDateTimeConverter();

            var time = DateTime.Parse(timestamp);

            converter.WriteJson(mockJsonWriter.Object, time, null);

            mockJsonWriter.Verify(m => m.WriteValue(unixTime), Times.Once);
        }

        [TestCase("2015-07-03T00:00:00", 1435881600L)]
        [TestCase("2015-12-30T21:00:51", 1451509251L)]
        public void unix_time_dates_are_deserialised_to_correct_dates(string timestamp, long unixTime)
        {
            var mockJsonReader = new Mock<JsonReader>();
            mockJsonReader.Setup(m => m.Value).Returns(unixTime);

            var converter = new UnixDateTimeConverter();

            var expectedTime = DateTime.Parse(timestamp);

            DateTime deserialisedValue = (DateTime)converter.ReadJson(mockJsonReader.Object, typeof(DateTime), null, null);

            Assert.That(deserialisedValue, Is.EqualTo(expectedTime));
        }
    }
}   

