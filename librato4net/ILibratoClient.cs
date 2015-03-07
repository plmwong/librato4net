using librato4net.Metrics;

namespace librato4net
{
    public interface ILibratoClient
    {
        void SendMetric(Metric metric);
    }
}

