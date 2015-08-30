using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace Logging.Client
{
    internal abstract class BaseLogger : ILog
    {
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

        protected LogEntity CreateLog(string source, string title, string message, Dictionary<string, string> tags, LogLevel level)
        {
            LogEntity log = new LogEntity();
            log.IP = ServerIP;
            log.Level = level;
            log.Message = message;
            log.Tags = tags;
            log.Time = Utils.GetTimeStamp(DateTime.Now);
            log.Title = title;
            log.Source = source;
            log.Thread = Thread.CurrentThread.ManagedThreadId;
            log.AppId = Convert.ToInt32(ConfigurationManager.AppSettings["AppId"] ?? "0");
            if (log.Tags == null)
            {
                log.Tags = new Dictionary<string, string>();
            }
            return log;
        }

        /// <summary>
        /// 是否禁用日志
        /// </summary>
        protected bool LoggingDisabled
        {
            get
            {
                bool LoggingDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LoggingDisabled"] ?? Settings.LoggingDisabled.ToString());
                return LoggingDisabled;
            }
        }

        protected abstract void Log(string title, string message, Dictionary<string, string> tags, LogLevel level);

      

        ///// <summary>
        ///// 获取客户端IP
        ///// </summary>
        ///// <returns></returns>
        //private static string GetClientIP()
        //{
        //    string str = string.Empty;
        //    if (HttpContext.Current != null)
        //    {
        //        IPAddress address;
        //        str = HttpContext.Current.Request.Headers["X-Forwarded-For"];
        //        if (string.IsNullOrWhiteSpace(str))
        //        {
        //            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //        }
        //        str = str.Trim().Split(new char[] { ',' })[0];
        //        if (IPAddress.TryParse(str, out address))
        //        {
        //            str = string.Empty;
        //        }
        //    }
        //    return str;
        //}

        private static string serverIP;

        private static string ServerIP
        {
            get
            {
                if (string.IsNullOrWhiteSpace(serverIP))
                {
                    serverIP = GetServerIP();
                }
                return serverIP;
            }
        }

        /// <summary>
        /// 获取服务器IP
        /// </summary>
        /// <returns></returns>
        private static string GetServerIP()
        {
            string str = "Did not get to the server IP";
            if (HttpContext.Current != null)
            {
                str = HttpContext.Current.Request.ServerVariables.Get("Local_Addr");
            }
            else
            {
                try
                {
                    string hostName = Dns.GetHostName();
                    var ipAddress = Dns.GetHostEntry(hostName)
                        .AddressList
                        .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                    if (ipAddress != null)
                    {
                        str = ipAddress.ToString();
                    }
                    return string.Empty;
                }
                catch (Exception) { }
            }
            return str;
        }
    }
}