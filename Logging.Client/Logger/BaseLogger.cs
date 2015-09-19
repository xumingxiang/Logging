using Logging.Client.LogSender;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Logging.Client
{
    internal abstract class BaseLogger : ILog
    {
        private static ITimerActionBlock<LogEntity> block;

        private string Source { get; set; }

        public BaseLogger(string source = "")
        {
            this.Source = source;
        }

        /// <summary>
        /// 是否启用日志
        /// </summary>
        protected static bool LoggingEnabled
        {
            get
            {
                bool LoggingDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LoggingEnabled"] ?? Settings.LoggingEnabled.ToString());
                return LoggingDisabled;
            }
        }

        static BaseLogger()
        {

            if (!LoggingEnabled) { return; }



            int LoggingTaskNum = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingTaskNum"] ?? Settings.LoggingTaskNum.ToString());

            int LoggingQueueLength = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingQueueLength"] ?? Settings.LoggingQueueLength.ToString());

            int LoggingBufferSize = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingBufferSize"] ?? Settings.LoggingBufferSize.ToString());

            int LoggingBlockElapsed = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingBlockElapsed"] ?? Settings.LoggingBlockElapsed.ToString());

            if (LoggingTaskNum <= 0) { LoggingTaskNum = Settings.DefaultLoggingTaskNum; }

            if (LoggingQueueLength <= 0) { LoggingQueueLength = Settings.DefaultLoggingQueueLength; }

            if (LoggingBufferSize <= 0) { LoggingBufferSize = Settings.DefaultLoggingBufferSize; }

            if (LoggingBlockElapsed <= 0) { LoggingBlockElapsed = Settings.DefaultLoggingBlockElapsed; }

            LogSenderBase sender = LogSenderManager.GetLogSender();

            if (LoggingTaskNum == 1)
            {

                block = new TimerActionBlock<LogEntity>((buffer) =>
                {
                    sender.Send(buffer);
                }, LoggingQueueLength, LoggingBufferSize, LoggingBlockElapsed);
            }
            else
            {
                block = new ThreadedTimerActionBlock<LogEntity>(LoggingTaskNum, (buffer) =>
                {
                    sender.Send(buffer);
                }, LoggingQueueLength, LoggingBufferSize, LoggingBlockElapsed);
            }
        }

        public void Debug(string message)
        {
            Debug(string.Empty, message);
        }

        public void Debug(string title, string message)
        {
            Debug(title, message, null);
        }

        public void Debug(string title, string message, Dictionary<string, string> tags)
        {
            Log(title, message, tags, LogLevel.Debug);
        }

        public void Info(string message)
        {
            Info(string.Empty, message, null);
        }

        public void Info(string title, string message)
        {
            Info(title, message, null);
        }

        public void Info(string title, string message, Dictionary<string, string> tags)
        {
            Log(title, message, tags, LogLevel.Info);
        }

        public void Warm(string message)
        {
            Warm(string.Empty, message);
        }

        public void Warm(string title, string message)
        {
            Warm(title, message, null);
        }

        public void Warm(string title, string message, Dictionary<string, string> tags)
        {
            Log(title, message, tags, LogLevel.Warm);
        }

        public void Error(string message)
        {
            Error(string.Empty, message, null);
        }

        public void Error(string title, string message)
        {
            Error(title, message, null);
        }

        public void Error(string title, string message, Dictionary<string, string> tags)
        {
            Log(title, message, tags, LogLevel.Error);
        }

        public void Error(Exception ex)
        {
            this.Error(ex.Message, GetExceptionMessage(ex));
        }

        private string GetExceptionMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.Source);
            sb.AppendLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                string msg = GetExceptionMessage(ex.InnerException);
                sb.AppendLine(msg);
            }
            return sb.ToString();
        }


        protected LogEntity CreateLog(string source, string title, string message, Dictionary<string, string> tags, LogLevel level)
        {
            LogEntity log = new LogEntity();
            log.Level = level;
            log.Message = message;
            log.Tags = tags;
            log.Time = Utils.GetTimeStamp(DateTime.Now);
            log.Title = title;
            log.Source = source;
            log.Thread = Thread.CurrentThread.ManagedThreadId;
            return log;
        }

        protected virtual void Log(string title, string message, Dictionary<string, string> tags, LogLevel level)
        {
            if (!LoggingEnabled) { return; }
            LogEntity log = this.CreateLog(Source, title, message, tags, level);
            block.Enqueue(log);
        }

        public string GetLogs(long start, long end, int appId, int[] level = null, string title = "", string msg = "", string source = "", string ip = "", Dictionary<string, string> tags = null, int limit = 100)
        {
            string loggingServerHost = ConfigurationManager.AppSettings["LoggingServerHost"];
            string url = loggingServerHost + "/LogViewer.ashx";

            StringBuilder query = new StringBuilder(url);
            query.Append("?start =" + start);
            query.Append("&");
            query.Append("end=" + end);
            query.Append("&");
            query.Append("appId=" + appId);
            if (level != null && level.Length > 0)
            {
                query.Append("&");
                query.Append("level=" + string.Join(",", level));
            }
            query.Append("&");
            query.Append("title=" + title);
            query.Append("&");
            query.Append("msg=" + msg);
            query.Append("&");
            query.Append("source=" + source);
            query.Append("&");
            query.Append("ip=" + ip);

            if (tags != null && tags.Count > 0)
            {
                string tags_str = string.Empty;
                foreach (var item in tags)
                {
                    tags_str += item.Key + "=" + item.Value;
                    tags_str += ",";
                }
                tags_str = tags_str.TrimEnd(',');
                query.Append("&");
                query.Append("tag=" + tags_str);
            }

            query.Append("&");
            query.Append("limit=" + limit);

            string resp = string.Empty;
            using (WebClient _client = new WebClient())
            {
                byte[] resp_byte = _client.DownloadData(query.ToString());
                resp = Encoding.UTF8.GetString(resp_byte);
            }
            return resp;
        }


    }
}