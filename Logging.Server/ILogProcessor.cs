using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Server
{
    internal interface ILogProcessor
    {
        void Process(IList<LogEntity> LogEntities);
    }
}
