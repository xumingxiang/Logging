using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Client
{
    internal abstract class LogSender
    {
        protected int SENDER_TIMEOUT = 5000;
        public abstract void Send(IList<LogEntity> LogEntities);
    }
}
