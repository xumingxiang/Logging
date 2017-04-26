using System.Collections.Generic;

namespace Logging.Server
{
    public class MetricEntity
    {
        public string Name { get; set; }

        public double Value { get; set; }

        public long Time { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }
}