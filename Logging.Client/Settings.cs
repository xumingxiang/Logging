using System;
using System.Configuration;

namespace Logging.Client
{
    public static class Settings
    {
    

        /// <summary>
        /// 设置日志服务器
        /// </summary>
        public static string LoggingServerHost { get; set; }

        /// <summary>
        /// 设置日志发送线程数。默认为1
        /// </summary>
        public static int LoggingTaskNum { get; set; }

        /// <summary>
        /// 设置日志队列最大长度
        /// </summary>
        public static int LoggingQueueLength { get; set; }

        /// <summary>
        /// 设置日志打包大小
        /// </summary>
        public static int LoggingBufferSize { get; set; }

        /// <summary>
        /// 设置日志发送阻塞时间。单位:毫秒
        /// </summary>
        public static int LoggingBlockElapsed { get; set; }

        /// <summary>
        /// 是否禁用日志
        /// </summary>
        public static bool LoggingEnabled { get; set; }

        /// <summary>
        /// 默认日志发送线程数：1
        /// </summary>
        public readonly static int DefaultLoggingTaskNum = 1;

        /// <summary>
        /// 应用号
        /// </summary>
        public readonly static int AppId = Convert.ToInt32(ConfigurationManager.AppSettings["AppId"] ?? "0");

        /// <summary>
        /// 默认日志队列最大长度：100000
        /// </summary>
        public readonly static int DefaultLoggingQueueLength = 100000;

        /// <summary>
        /// 默认日志打包大小：300
        /// </summary>
        public readonly static int DefaultLoggingBufferSize = 300;

        /// <summary>
        /// 默认发送阻塞时间。单位:毫秒。5000,即10秒
        /// </summary>
        public readonly static int DefaultLoggingBlockElapsed = 5000;
    }
}