using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.Viewer
{
   public interface ILogViewer
    {
        List<LogEntity> GetLogs(long start, long end, int appId, int[] level, string title, string msg, string source, int ip, int limit = 100);
    }
}
