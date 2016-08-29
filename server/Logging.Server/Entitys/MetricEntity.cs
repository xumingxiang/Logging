using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
