using System;
using System.Collections.Generic;

namespace Logging.Server.Viewer
{
    internal class HBaseViewer : ILogViewer
    {
        public List<LogEntity> GetLogs(long start, long end, int appId, int[] level, string title, string msg, string source, int ip,List<string>tags, int limit = 100)
        {
            return null;
        }

        public List<LogStatistics> GetStatistics(long start, long end,int appId)
        {
            throw new NotImplementedException();
        }
    }
}