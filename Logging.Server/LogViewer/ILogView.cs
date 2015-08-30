using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.LogViewer
{
    interface ILogViewer
    {
        List<LogEntity> GetLogs(DateTime start, DateTime end, int appId, int[] level, string title, string msg, string source, string ip, int limit = 100);
    }
}
