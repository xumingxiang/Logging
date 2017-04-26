using Logging.Server.Alerting;
using System;
using System.Collections.Generic;

namespace Logging.Server.Writer
{
    public class HBaseWriter : ILogWriter
    {
        public void RecordAlerting(AlertingHistory ah)
        {
            throw new NotImplementedException();
        }

        public void SetLogOnOff(LogOnOff on_off)
        {
            throw new NotImplementedException();
        }

        public void SetOptions(List<Options> opts)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<LogEntity> logs)
        {
            throw new NotImplementedException();
        }
    }
}