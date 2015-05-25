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

        public int Level { get; set; }

        public Dictionary<string,string> Tags { get; set; }

    }
}
