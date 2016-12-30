using System;

namespace Logging.Client
{
    internal class LogSenderResult
    {
        public bool Success { get; set; }

        public Exception Exception { get; set; }
    }
}