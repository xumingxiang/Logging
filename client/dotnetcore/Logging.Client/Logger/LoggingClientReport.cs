using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Logging.Client
{
    /// <summary>
    /// 处理日志过程中出现的异常
    /// </summary>
    internal class LoggingClientReport
    {
        private static ILog logger = LogManager.GetLogger(typeof(LoggingClientReport));

        private static FileLogger fileLogger = new FileLogger(Settings.AppId);

        private static readonly string ver = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

        /// <summary>
        /// 报告异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="count"></param>
        public static void ReportException(Exception ex, int count)
        {
            try
            {
                if (count <= 0) { return; }
                StringBuilder msg_sb = new StringBuilder();

                msg_sb.AppendLine("最近一分钟内该应用(" + Settings.AppId + ")Logging.Client发生" + count + "条异常数量");
                msg_sb.AppendLine("最近一条异常:" + ex.ToString());
                msg_sb.AppendLine("ThreadId:" + Thread.CurrentThread.ManagedThreadId);
                msg_sb.AppendLine("HostIP:" + Utils.GetServerIP());
                msg_sb.AppendLine("Ver:" + ver);
                msg_sb.AppendLine("ReportTime:" + DateTime.Now);

                var tags = new Dictionary<string, string>();
                tags.Add("type", "one_minute_err");
                tags.Add("ver", ver);

                //  logger.Error("Logging_Client_Report", msg_sb.ToString(), tags);
                logger.Metric("logging_client_err", count);
                fileLogger.Log(msg_sb.ToString());
            }
            catch { }
        }

        /// <summary>
        /// 报告溢出
        /// </summary>
        /// <param name="over_count"></param>
        /// <param name="currentQueneLength"></param>
        /// <param name="maxQueueLength"></param>
        public static void ReportOver(int over_count, int currentQueneLength, int maxQueueLength)
        {
            try
            {
                if (over_count <= 0) { return; }

                StringBuilder msg_sb = new StringBuilder();
                msg_sb.AppendLine("最近一分钟内该应用(" + Settings.AppId + ")发生Logging_Client_Over溢出数量:" + over_count + " 。 建议增加 LoggingQueueLength 和配置值。");
                msg_sb.AppendLine("CurrentQueneLength:" + currentQueneLength);
                msg_sb.AppendLine("MaxQueueLength:" + maxQueueLength);
                msg_sb.AppendLine("ThreadId:" + Thread.CurrentThread.ManagedThreadId);
                msg_sb.AppendLine("HostIP:" + Utils.GetServerIP());
                msg_sb.AppendLine("Ver:" + ver);
                msg_sb.AppendLine("ReportTime:" + DateTime.Now);

                var over_log_tags = new Dictionary<string, string>();
                over_log_tags.Add("type", "logging_client_over");

                //    logger.Error("Logging_Client_Report", msg_sb.ToString(), over_log_tags);
                fileLogger.Log(msg_sb.ToString());
            }
            catch { }
        }

        ///// <summary>
        ///// 报告传输数据Size溢出
        ///// </summary>
        //public static void ReportTransportOver(TTransportDataSizeOverflowException tdoe)
        //{
        //    try
        //    {
        //        if (tdoe == null) { return; }
        //        var over_log_tags = new Dictionary<string, string>();
        //        over_log_tags.Add("type", "logging_client_trans_over");
        //       // logger.Error("Logging_Client_Report", tdoe.Message, over_log_tags);
        //        fileLogger.Log(tdoe);
        //    }
        //    catch { }
        //}
    }
}