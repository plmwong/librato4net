using librato4net.Annotations;
using librato4net.Metrics;

namespace librato4net
{
    public interface ILibratoClient
    {
        void SendMetric(Metric metric);
        void SendAnnotation(Annotation annotation);
    }
}

