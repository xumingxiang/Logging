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
        public static int LoggingBatchSize { get; set; }

        /// <summary>
        /// 设置日志发送阻塞时间。单位:毫秒
        /// </summary>
        public static int LoggingBlockElapsed { get; set; }

        /// <summary>
        /// 是否禁用日志
        /// </summary>
        public static bool LoggingDisabled { get; set; }


      
        /// <summary>
        /// Default设置日志发送线程数。默认为1
        /// </summary>
        public static int DefaultLoggingTaskNum { get { return 1; } }

        /// <summary>
        /// Default设置日志队列最大长度。默认为10000
        /// </summary>
        public static int DefaultLoggingQueueLength { get { return 10000; } }

        /// <summary>
        /// Default设置日志打包大小。默认为100
        /// </summary>
        public static int DefaultLoggingBatchSize { get { return 100; } }

        /// <summary>
        /// Default设置日志发送阻塞时间。单位:毫秒,。默认为10000,即10秒
        /// </summary>
        public static int DefaultLoggingBlockElapsed { get { return 10000; } }


    }
}