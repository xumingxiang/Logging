using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Server
{
    internal sealed class LogProcessorManager
    {
        private LogProcessorManager()
        { }

        public static ILogProcessor GetLogProcessor()
        {
            return new LogProcessor();
        }
    }
}
