using System;
using System.Collections.Generic;
using System.Configuration;

namespace Logging.Client
{
    internal class SimpleLogger : BaseLogger, ILog
    {
        //public string Source { get; set; }

        public SimpleLogger(string source) 
            : base(source)
        {

        }

        

        //protected override void Log(string title, string message, Dictionary<string, string> tags, LogLevel level)
        //{
        //    if (LoggingDisabled) { return; }
        //    LogEntity log = base.CreateLog(Source, title, message, tags, level);
        //    block.Enqueue(log);
        //}

        //private static TimerBatchBlock<LogEntity> block;

        //static SimpleLogger()
        //{
        //    int LoggingTaskNum = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingTaskNum"] ?? Settings.LoggingTaskNum.ToString());

        //    int LoggingQueueLength = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingQueueLength"] ?? Settings.LoggingQueueLength.ToString());

        //    int LoggingBatchSize = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingBatchSize"] ?? Settings.LoggingBatchSize.ToString());

        //    int LoggingBlockElapsed = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingBlockElapsed"] ?? Settings.LoggingBlockElapsed.ToString());

        //    if (LoggingTaskNum <= 0) { LoggingTaskNum = Settings.DefaultLoggingTaskNum; }

        //    if (LoggingQueueLength <= 0) { LoggingQueueLength = Settings.DefaultLoggingQueueLength; }

        //    if (LoggingBatchSize <= 0) { LoggingBatchSize = Settings.DefaultLoggingBatchSize; }

        //    if (LoggingBlockElapsed <= 0) { LoggingBlockElapsed = Settings.DefaultLoggingBlockElapsed; }

        //    block = new TimerBatchBlock<LogEntity>(LoggingTaskNum, (batch) =>
        //         {
        //             LogSenderBase sender = new HttpLogSender();
        //             sender.Send(batch);
        //         }, LoggingQueueLength, LoggingBatchSize, LoggingBlockElapsed);
        //}
    }
}