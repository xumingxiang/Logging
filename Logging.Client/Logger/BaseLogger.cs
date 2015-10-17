using Logging.Client.LogSender;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading;

namespace Logging.Client
{
    internal abstract class BaseLogger : ILog
    {
        private static ITimerActionBlock<ILogEntity> block;

        private string Source { get; set; }

        public BaseLogger(string source = "")
        {
            this.Source = source;
        }

        /// <summary>
        /// 客户端是否启用日志
        /// </summary>
        private readonly static bool LoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LoggingEnabled"] ?? Settings.LoggingEnabled.ToString());


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
                block = new TimerActionBlock<ILogEntity>((buffer) =>
                {
                    sender.Send(buffer);
                }, LoggingQueueLength, LoggingBufferSize, LoggingBlockElapsed);
            }
            else
            {
                block = new ThreadedTimerActionBlock<ILogEntity>(LoggingTaskNum, (buffer) =>
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
            this.Error(ex, null);
        }


        public void Error(string title, Exception ex)
        {
            this.Error(title, GetExceptionMessage(ex), null);
        }

        public void Error(Exception ex, Dictionary<string, string> tags)
        {
            this.Error(ex.Message, GetExceptionMessage(ex), tags);
        }


        public void Error(string title, Exception ex, Dictionary<string, string> tags)
        {
            this.Error(title, GetExceptionMessage(ex), tags);
        }

        private string GetExceptionMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.Source);
            sb.AppendLine(ex.StackTrace);
            //if (ex.TargetSite != null)
            //{
            //    sb.AppendLine(ex.TargetSite.Name);
            //}
            if (ex.InnerException != null)
            {
                string msg = GetExceptionMessage(ex.InnerException);
                sb.AppendLine(msg);
            }
            return sb.ToString();
        }

        public void Metric(string name, double value, Dictionary<string, string> tags = null)
        {
            if (!LoggingEnabled) { return; }

            var Metric = new MetricEntity();
            Metric.Name = name;
            Metric.Value = value;
            Metric.Tags = tags;
            Metric.Time = Utils.GetUnixTime(DateTime.Now);
            block.Enqueue(Metric);
            //PrivatePoint(name, value, tags);
            //SysPoint();
        }


        protected LogEntity CreateLog(string source, string title, string message, Dictionary<string, string> tags, LogLevel level)
        {
            LogEntity log = new LogEntity();
            log.Level = level;
            log.Message = message;
            log.Tags = tags;
            log.Time = Utils.GetTimeTicks(DateTime.Now);
            log.Title = title;
            log.Source = source;
            log.Thread = Thread.CurrentThread.ManagedThreadId;
            return log;
        }



        protected virtual void Log(string title, string message, Dictionary<string, string> tags, LogLevel level)
        {
            if (!LoggingEnabled) { return; }

            LogOnOff onOff = GetLogOnOff();

            if (level == LogLevel.Debug && onOff.Debug != 1) { return; }
            if (level == LogLevel.Info && onOff.Info != 1) { return; }
            if (level == LogLevel.Warm && onOff.Warm != 1) { return; }
            if (level == LogLevel.Error && onOff.Error != 1) { return; }

            LogEntity log = this.CreateLog(Source, title, message, tags, level);
            block.Enqueue(log);
        }

        public string GetLogs(long start, long end, int appId, int[] level = null, string title = "", string msg = "", string source = "", string ip = "", Dictionary<string, string> tags = null, int limit = 100)
        {

            string url = Settings.LoggingServerHost + "/LogViewer.ashx";

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

        public void DebugWithTags(string title, string message, string[] tags)
        {
            var tags_dic = new Dictionary<string, string>();

            foreach (var tag in tags)
            {
                string tag_key = tag.Split('=')[0];
                string tag_val = tag.Substring(tag_key.Length, tag.Length - tag_key.Length);
                tags_dic[tag_key] = tag_val;
            }

            this.Debug(title, message, tags_dic);
        }

        public void InfoWithTags(string title, string message, string[] tags)
        {
            var tags_dic = new Dictionary<string, string>();

            foreach (var tag in tags)
            {
                string tag_key = tag.Split('=')[0];
                string tag_val = tag.Substring(tag_key.Length, tag.Length - tag_key.Length);
                tags_dic[tag_key] = tag_val;
            }

            this.Info(title, message, tags_dic);
        }

        public void WarmWithTags(string title, string message, string[] tags)
        {
            var tags_dic = new Dictionary<string, string>();

            foreach (var tag in tags)
            {
                string tag_key = tag.Split('=')[0];
                string tag_val = tag.Substring(tag_key.Length, tag.Length - tag_key.Length);
                tags_dic[tag_key] = tag_val;
            }

            this.Warm(title, message, tags_dic);
        }

        public void ErrorWithTags(string title, string message, string[] tags)
        {
            var tags_dic = new Dictionary<string, string>();

            foreach (var tag in tags)
            {
                string tag_key = tag.Split('=')[0];
                string tag_val = tag.Substring(tag_key.Length, tag.Length - tag_key.Length);
                tags_dic[tag_key] = tag_val;
            }

            this.Error(title, message, tags_dic);
        }


        private static string LogOnOffCackeKey = "LoggingClient_LogOnOff";

        private static int LogOnOffCackeTimeOut = 10;//单位:分钟

        private static string GetLogOnOffUrl = Settings.LoggingServerHost + "/GetLogOnOff.ashx?appId=" + Settings.AppId;

        /// <summary>
        /// 获取服务端日志开关,10分钟缓存
        /// </summary>
        /// <returns></returns>
        private static LogOnOff GetLogOnOff()
        {
            LogOnOff logOnOff = MemoryCache.Default.Get(LogOnOffCackeKey) as LogOnOff;
            if (logOnOff == null)
            {
               
                string resp = string.Empty;
                using (WebClient _client = new WebClient())
                {
                    byte[] resp_byte = _client.DownloadData(GetLogOnOffUrl);
                    resp = Encoding.UTF8.GetString(resp_byte);
                }
                if (!string.IsNullOrWhiteSpace(resp))
                {
                    logOnOff = new LogOnOff();
                    string[] arr = resp.Split(',');
                    logOnOff.Debug = Convert.ToByte(arr[0]);
                    logOnOff.Info = Convert.ToByte(arr[1]);
                    logOnOff.Warm = Convert.ToByte(arr[2]);
                    logOnOff.Error = Convert.ToByte(arr[3]);
                    MemoryCache.Default.Add(LogOnOffCackeKey, logOnOff, DateTimeOffset.Now.AddMinutes(LogOnOffCackeTimeOut));
                }
            }

            if (logOnOff == null)
            {
                logOnOff = new LogOnOff();
                logOnOff.Debug = 1;
                logOnOff.Info = 1;
                logOnOff.Warm = 1;
                logOnOff.Error = 1;
            }
            return logOnOff;
        }
    }
}