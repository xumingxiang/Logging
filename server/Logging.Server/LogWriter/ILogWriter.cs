using Logging.Server.Alerting;
using System.Collections.Generic;

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