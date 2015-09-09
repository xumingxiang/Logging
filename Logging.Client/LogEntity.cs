using System.Collections.Generic;

namespace Logging.Client
{
    internal class LogEntity
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public long IP { get; set; }

        public LogLevel Level { get; set; }

        public int AppId { get; set; }

        public long Time { get; set; }

        public string Source { get; set; }

        public int Thread { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }
}