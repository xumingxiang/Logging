using Logging.Server.Processor;
using System;
using System.Collections.Generic;
using System.Configuration;
using Logging.ThriftContract;
using MongoDB.Bson;

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
            var processor = LogProcessorManager.GetLogProcessor();
            processor.Process(logs);
        }

        public void Log(List<TLogEntity> logEntities)
        {
            IList<LogEntity> _logEntities = new List<LogEntity>();
            foreach (var item in logEntities)
            {
                List<string> tags = new List<string>();

                if (item.Tags != null && item.Tags.Count > 0)
                {
                    foreach (var tag in item.Tags)
                    {
                        tags.Add(tag.Key.Replace("=", string.Empty) + "=" + tag.Value);
                    }
                }

                LogEntity _log = new LogEntity();
                _log.IP = item.IP;
                _log.Level = (LogLevel)item.Level;
                _log.Message = item.Message;
                _log.Tags = tags;
                _log.Title = item.Title;
                _log.Source = item.Source;
                _log.Thread = item.Thread;
                _log.Time = item.Time;
                _log.AppId = item.AppId;
                //_log._id = _log.CreateObjectId();
                _logEntities.Add(_log);
            }
            queue.Enqueue(_logEntities);
        }
    }
}
