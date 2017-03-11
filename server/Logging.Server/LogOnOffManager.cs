using Logging.Server.Viewer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;
using Logging.Server.Writer;

namespace Logging.Server
{
    public sealed class LogOnOffManager
    {
        private LogOnOffManager()
        {
        }

        private readonly static int LogOnOffCacheTimeOut = 10;//单位:分钟

        private static DateTime LastUpdateTime;

        /// <summary>
        /// 默认开关常量
        /// </summary>
        private readonly static LogOnOff Default = new LogOnOff { Debug = 1, Error = 1, Info = 1, Warn = 1 };

        private readonly static object lockthis = new object();

        /// <summary>
        /// 全部开关
        /// </summary>
        private static List<LogOnOff> ALL_LOG_ONOFF = null;

        /// <summary>
        /// 获取指定开关
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static LogOnOff GetLogOnOff(int appId)
        {
            LogOnOff logOnOff = GetALLLogOnOff().FirstOrDefault(x => x.AppId == appId);
            if (logOnOff == null) { return Default; }
            return logOnOff;
        }

        /// <summary>
        /// 获取所有开关
        /// </summary>
        /// <returns></returns>
        public static List<LogOnOff> GetALLLogOnOff()
        {
            if (ALL_LOG_ONOFF == null || (DateTime.Now - LastUpdateTime).TotalMilliseconds > LogOnOffCacheTimeOut)
            {
                lock (lockthis)
                {
                    if (ALL_LOG_ONOFF == null || (DateTime.Now - LastUpdateTime).TotalMilliseconds > LogOnOffCacheTimeOut)
                    {
                        RefreshLogOnOff();
                      
                    }
                }
            }
            return ALL_LOG_ONOFF;
        }

        /// <summary>
        /// 刷新所有开关
        /// </summary>
        private static void RefreshLogOnOff()
        {
            ALL_LOG_ONOFF = LogViewerManager.GetLogViewer().GetALLLogOnOff();
            LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 设置指定开关
        /// </summary>
        /// <param name="on_off"></param>
        public static void SetLogOnOff(LogOnOff on_off)
        {
            LogWriterManager.GetLogWriter().SetLogOnOff(on_off);
            RefreshLogOnOff();
        }
    }
}