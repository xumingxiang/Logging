using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging.Server.Alerting;

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
