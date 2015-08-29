using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Server
{
    public class LogReciver : LogTransferService.Iface
    {

        private static BlockingActionQueue<IList<LogEntity>> queue;

        public LogReciver() { 
        
        }

        static LogReciver()
        {
            queue = new BlockingActionQueue<IList<LogEntity>>(3, (LogEntities) =>
            {
                ProcessLog(LogEntities);
            }, 2000);
        }

        private static void ProcessLog(IList<LogEntity> LogEntities)
        {
            // write log to db
            var logProcessor = LogProcessorManager.GetLogProcessor();
            logProcessor.Process(LogEntities);
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
                _logEntities.Add(_log);
            }
            queue.Enqueue(_logEntities);
        }
    }
}
