using Logging.Server.Metric.Writer;
using System.Collections.Generic;

namespace Logging.Server.Metric.Processor
{
    /// <summary>
    /// 处理队列元素(日志)的处理器
    /// </summary>
    public class DefaulMetricProcessor : IMetricProcessor
    {
        public void Process(IList<MetricEntity> logs)
        {
            InfluxdbReport report = new InfluxdbReport();

            report.Write(logs);
        }
    }
}