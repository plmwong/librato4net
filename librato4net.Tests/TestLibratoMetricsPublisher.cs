using System.Collections.Specialized;

namespace librato4net.Tests
{
    public class TestLibratoMetricsPublisher : LibratoMetricsPublisher
    {
        public NotifyCollectionChangedEventArgs EventArgs { get; private set; }
        public bool CountsChangedCalled { get; private set; }

        public TestLibratoMetricsPublisher(ILibratoClient libratoClient)
            : base(libratoClient)
        {
        }

        protected override void CountsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            EventArgs = e;
            CountsChangedCalled = true;
        }
    }
}

