using Logging.Server.Processor;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Logging.Server
{
    public class LogReciver : LogTransferService.Iface
    {

        private static BlockingActionQueue<IList<LogEntity>> queue;

        public LogReciver()
        {

        }

        static LogReciver()
        {
            int processTaskNum = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessTaskNum"]);
            int blockingQueueLength = Convert.ToInt32(ConfigurationManager.AppSettings["BlockingQueueLength"]);

            queue = new BlockingActionQueue<IList<LogEntity>>(processTaskNum, (LogEntities) =>
            {
                ProcessLog(LogEntities);
            }, blockingQueueLength);
        }

        private static void ProcessLog(IList<LogEntity> logs)
        {
            for (int i = 0; i < logs.Count; i++)
            {
                logs[i].CreateTime = DateTime.Now;
            }

            // write log to db
            var logProcessor = LogProcessorManager.GetLogProcessor();
            logProcessor.Process(logs);
        }

        public void Log(List<global::LogEntity> logEntities)
        {
            IList<LogEntity> _logEntities = new List<LogEntity>();
            foreach (var item in logEntities)
            {
                LogEntity _log = new LogEntity();
                _log.IP = item.IP;
                _log.Level = (LogLevel)item.Level;
                _log.Message = item.Message;
                _log.Tags = item.Tags;
                _log.Title = item.Title;
                _log.Source = item.Source;
                _log.Thread = item.Thread;
                _log.Time = item.Time;
                _log.AppId = item.AppId;
                _logEntities.Add(_log);
            }
            queue.Enqueue(_logEntities);
        }
    }
}
