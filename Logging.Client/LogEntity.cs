using System.Collections.Generic;

namespace Logging.Client
{
    
    public class LogEntity
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public LogLevel Level { get; set; }

        public long Time { get; set; }

        public string Source { get; set; }

        public int Thread { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }

   
}