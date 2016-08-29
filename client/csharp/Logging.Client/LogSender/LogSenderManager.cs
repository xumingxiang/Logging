using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Client.LogSender
{
    internal sealed class LogSenderManager
    {
        private LogSenderManager() { }

        public static LogSenderBase GetLogSender()
        {
            return new THttpLogSender();
        }

    }
}
