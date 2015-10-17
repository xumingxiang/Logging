using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.Viewer
{
   public interface ILogViewer
    {
        List<LogEntity> GetLogs(long start, long end, int appId, int[] level, string title, string msg, string source, int ip,List<string>tags, int limit = 100);

        List<LogStatistics> GetStatistics(long start, long end,int appId);

        /// <summary>
        /// 获取所有日志开关
        /// </summary>
        /// <returns></returns>
        List<LogOnOff> GetALLLogOnOff();

        /// <summary>
        /// 获取指定AppId的日志开关
        /// </summary>
        /// <returns></returns>    
        LogOnOff GetLogOnOff(int appId);
    }
}
