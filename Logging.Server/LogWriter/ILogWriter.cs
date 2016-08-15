using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging.Server.Alerting;

namespace Logging.Server.Writer
{
   public interface ILogWriter
    {
         void Write(IList<LogEntity> logs);

        void SetLogOnOff(LogOnOff on_off);
        void SetOptions(List<Options> opts);
        void RecordAlerting(AlertingHistory ah);
    }
}
