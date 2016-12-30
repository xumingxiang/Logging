using System.Collections.Generic;

namespace Logging.Client
{
    internal interface ILogEntity
    {
        /// <summary>
        /// 类型:1Log;2:Metric
        /// </summary>
        int Type { get; set; }
    }

    public class LogEntity : ILogEntity
    {
        public int Type { get; set; }

        public LogEntity()
        {
            this.Type = 1;
        }

        public string Title { get; set; }

        public string Message { get; set; }

        public LogLevel Level { get; set; }

        public long Time { get; set; }

        public string Source { get; set; }

        public int Thread { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }

    public class MetricEntity : ILogEntity
    {
        public MetricEntity()
        {
            this.Type = 2;
        }

        public int Type { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public long Time { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }
}