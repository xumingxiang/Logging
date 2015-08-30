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
        /// 设置日志发送阻塞时间
        /// </summary>
        public static int LoggingBlockElapsed { get; set; }

        /// <summary>
        /// 是否禁用日志
        /// </summary>
        public static bool LoggingDisabled { get; set; }
    }
}