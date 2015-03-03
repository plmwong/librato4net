namespace librato4net
{
    public class LibratoMetricsPublisher : MetricsPublisher
    {
        internal override void Measure(string metricName, object value)
        {
            throw new System.NotImplementedException();
        }

        internal override void Increment(string metricName, int @by)
        {
            throw new System.NotImplementedException();
        }
    }
}