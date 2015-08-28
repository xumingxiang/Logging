using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Server
{
    internal class LogEntity
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string IP { get; set; }

        public LogLevel Level { get; set; }

        public int AppId { get; set; }

        public long TimeSpan { get; set; }

        public string Source { get; set; }

        public int Thread { get; set; }

        public Dictionary<string, string> Tags { get; set; }

    }
}
