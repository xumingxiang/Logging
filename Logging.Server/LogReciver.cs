using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Server
{
    internal class LogReciver
    {

        private static BlockingActionQueue<IList<LogEntity>> queue;

        static LogReciver()
        {
            queue = new BlockingActionQueue<IList<LogEntity>>(3, (LogEntities) =>
            {
                //Thread.Sleep(10);//假设处理逻辑花了10毫秒
                // Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.ffff"));
                ProcessLog(LogEntities);
            }, 2000);
        }

        private static void ProcessLog(IList<LogEntity> LogEntities)
        {
            // write log to db
            var logProcessor = LogProcessorManager.GetLogProcessor();
            logProcessor.Process(LogEntities);
        }
    }
}
