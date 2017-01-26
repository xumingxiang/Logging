using Logging.Server.Metric.Writer;
using System.Collections.Generic;
using System.Configuration;

namespace Logging.Server.Metric.Processor
{
    /// <summary>
    /// 处理队列元素(日志)的处理器
    /// </summary>
    public class DefaulMetricProcessor : IMetricProcessor
    {
       // private static string influxdbVer = ConfigurationManager.AppSettings["MetricInfluxdbVer"];

        public void Process(IList<MetricEntity> logs)
        {
            if (Config.MetricInfluxdbVer == "0.8")
            {
                InfluxdbReport report = new InfluxdbReport();
                report.WriteAsync(logs);
            }
            else
            {
                InfluxdbWriter report = new InfluxdbWriter();
                report.WriteAsync(logs);
            }
        }
    }
}