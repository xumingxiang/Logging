using Logging.Client.LogSender;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace Logging.Client
{
    internal abstract partial class BaseLogger : ILog
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


        private static LogSenderBase sender = LogSenderManager.GetLogSender();

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



            if (LoggingTaskNum == 1)
            {
                block = new TimerActionBlock<ILogEntity>((buffer) =>
                {
                    Send(buffer);
                }, LoggingQueueLength, LoggingBufferSize, LoggingBlockElapsed);
            }
            else
            {
                block = new ThreadedTimerActionBlock<ILogEntity>(LoggingTaskNum, (buffer) =>
                {
                    Send(buffer);
                }, LoggingQueueLength, LoggingBufferSize, LoggingBlockElapsed);
            }
        }

        private static void Send(IList<ILogEntity> buffer)
        {
            long size = sender.Send(buffer);
            LogOnOffManager.RefreshLogOnOff();
            PrivateMetric("logging_client_send_data_size", size);
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

        public void Warn(string message)
        {
            Warn(string.Empty, message);
        }

        public void Warn(string title, string message)
        {
            Warn(title, message, null);
        }

        public void Warn(string title, string message, Dictionary<string, string> tags)
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
            this.Error(title, ex, null);
        }

        public void Error(Exception ex, Dictionary<string, string> tags)
        {
            this.Error(ex.Message, ex, tags);
        }

        public void Error(string title, Exception ex, Dictionary<string, string> tags)
        {
            string err_msg = string.Empty;
            if (HttpContext.Current == null)
            {
                err_msg = new Error(ex, null).ToString();
            }
            else
            {
                try
                {
                    err_msg = new Error(ex, new HttpContextWrapper(HttpContext.Current)).ToString();
                }
                catch
                {
                    err_msg = new Error(ex, null).ToString();
                }
            }
            this.Error(title, err_msg, tags);
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

        private static void PrivateMetric(string name, double value, Dictionary<string, string> tags = null, DateTime time = default(DateTime))
        {
            if (!LoggingEnabled) { return; }
            var Metric = new MetricEntity();
            Metric.Name = name;
            Metric.Value = value;
            Metric.Tags = tags;
            if (time == default(DateTime))
            {
                Metric.Time = Utils.GetUnixTime(DateTime.Now);
            }
            else
            {
                Metric.Time = Utils.GetUnixTime(time);
            }
            block.Enqueue(Metric);
        }

        public void Metric(string name, double value, Dictionary<string, string> tags = null)
        {
            PrivateMetric(name, value, tags);
        }



        public void Metric(string name, Dictionary<string, string> tags = null)
        {
            Metric(name, 1, tags);
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
            if (log.Tags == null)
            {
                log.Tags = new Dictionary<string, string>();
            }
            log.Tags.Add("traceid", Trace.CorrelationManager.ActivityId.ToString("N"));
            return log;
        }

        protected virtual void Log(string title, string message, Dictionary<string, string> tags, LogLevel level)
        {
            if (!LoggingEnabled) { return; }

            LogOnOff onOff = LogOnOffManager.GetLogOnOff();

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

        public void WarnWithTags(string title, string message, string[] tags)
        {
            var tags_dic = new Dictionary<string, string>();

            foreach (var tag in tags)
            {
                string tag_key = tag.Split('=')[0];
                string tag_val = tag.Substring(tag_key.Length, tag.Length - tag_key.Length);
                tags_dic[tag_key] = tag_val;
            }

            this.Warn(title, message, tags_dic);
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

        public void Metric(string name, double value, DateTime time, Dictionary<string, string> tags = null)
        {
            PrivateMetric(name, value, tags, time);
        }

        public void Flush()
        {
            block.Flush();
        }
    }
}