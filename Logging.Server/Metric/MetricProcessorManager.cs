namespace Logging.Server.Metric.Processor
{
    internal sealed class MetricProcessorManager
    {
        private MetricProcessorManager()
        { }

        public static IMetricProcessor GetMetricProcessor()
        {
            return new DefaulMetricProcessor();
        }
    }
}