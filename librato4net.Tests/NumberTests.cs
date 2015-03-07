using NUnit.Framework;

namespace librato4net.Tests
{
    [TestFixture]
    public class NumberTests
    {
        [Test]
        public void integer_can_be_placed_into_number()
        {
            Number number = 24;

            Assert.That(number.IntegerValue, Is.EqualTo(24));
            Assert.That(number.FloatValue, Is.Null);
        }

        [Test]
        public void float_can_be_placed_into_number()
        {
            Number number = 45.0f;

            Assert.That(number.FloatValue, Is.EqualTo(45.0f));
            Assert.That(number.IntegerValue, Is.Null);
        }
    }
}

