using System;
using System.Collections.Generic;
using Logging.Server.Alerting;

namespace Logging.Server.Viewer
{
    internal class HBaseViewer : ILogViewer
    {
        public List<LogOnOff> GetALLLogOnOff()
        {
            throw new NotImplementedException();
        }

        public AlertingHistory GetLastAlertingHistory(int appId, AlertingType appError)
        {
            throw new NotImplementedException();
        }

        public LogOnOff GetLogOnOff(int appId)
        {
            throw new NotImplementedException();
        }

        public List<LogEntity> GetLogs(long start, long end, int appId, int[] level, string title, string msg, string source, long ip,List<string>tags, int limit = 100)
        {
            return null;
        }

        public List<Options> GetOptions(string[] optKeys)
        {
            throw new NotImplementedException();
        }

        public List<LogStatistics> GetStatistics(long start, long end,int appId)
        {
            throw new NotImplementedException();
        }
    }
}