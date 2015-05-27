using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Logging.Client
{
    internal class SimpleLogger : ILog
    {
        public string Source { get; set; }

        public SimpleLogger(string source = "")
        {
            this.Source = source;
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

        private void Log(string title, string message, Dictionary<string, string> tags, LogLevel level)
        {
            LogEntity log = new LogEntity();
            log.IP = GetClientIP();
            log.Level = level;
            log.Message = message;
            log.Tags = tags;
            log.Time = GetTimeStamp(DateTime.Now);
            log.Title = title;

            if (log.Tags == null)
            {
                log.Tags = new Dictionary<string, string>();
            }
            log.Tags.Add("Source", this.Source);

            b.Enqueue(log);

        }
        static TimerBatchBlock<LogEntity> b;
        static SimpleLogger()
        {
            b = new TimerBatchBlock<LogEntity>(1, (batch) =>
             {
                 LogSender sender = new HttpLogSender();
                 sender.Send(batch);

             }, 1000, 10, 100);
        }



        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name=”time”></param>
        /// <returns></returns>
        private static long GetTimeStamp(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).Ticks / 10000;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        private static string GetClientIP()
        {
            string str = string.Empty;
            if (HttpContext.Current != null)
            {
                IPAddress address;
                str = HttpContext.Current.Request.Headers["X-Forwarded-For"];
                if (string.IsNullOrWhiteSpace(str))
                {
                    return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                str = str.Trim().Split(new char[] { ',' })[0];
                if (IPAddress.TryParse(str, out address))
                {
                    str = string.Empty;
                }
            }
            return str;
        }

    }
}
