using System;

namespace Logging.Client
{
    /// <summary>
    /// 处理日志过程中出现的异常
    /// </summary>
    internal class LogExceptionHandller
    {
       static ILog logger = LogManager.GetLogger(typeof(LogExceptionHandller));

        public static void WriteLog(Exception ex)
        {
            logger.Error("Logging_Client_Exception", ex.ToString());
        }
    }
}