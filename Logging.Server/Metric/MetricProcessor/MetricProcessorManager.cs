namespace Logging.Server.Metric.Processor
{
    public sealed class MetricProcessorManager
    {
        private MetricProcessorManager()
        { }

        public static IMetricProcessor GetMetricProcessor()
        {
            return new DefaulMetricProcessor();
        }
    }
}