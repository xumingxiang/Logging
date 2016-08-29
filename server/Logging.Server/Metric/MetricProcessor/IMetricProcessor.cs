using System.Collections.Generic;

namespace Logging.Server.Metric.Processor
{
    /// <summary>
    /// 处理队列元素(日志)的处理器
    /// </summary>
    public interface IMetricProcessor
    {
        void Process(IList<MetricEntity> logs);
    }
}