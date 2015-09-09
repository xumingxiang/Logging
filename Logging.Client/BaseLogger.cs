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

        public void Error(Exception ex)
        {
            this.Error(ex.Message, ex.ToString());
        }

        protected LogEntity CreateLog(string source, string title, string message, Dictionary<string, string> tags, LogLevel level)
        {
            LogEntity log = new LogEntity();
            log.IP = ServerIPNum;
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

        private static long serverIPNum;

        private static long ServerIPNum
        {
            get
            {
                if (serverIPNum<=0)
                {
                   string serverIP = GetServerIP();
                    serverIPNum = Utils.IPToNumber(serverIP);
                }
                return serverIPNum;
            }
        }

        /// <summary>
        /// 获取服务器IP
        /// </summary>
        /// <returns></returns>
        private static string GetServerIP()
        {
            string str = "127.0.0.1";
            //if (HttpContext.Current != null)
            //{
            //    str = HttpContext.Current.Request.ServerVariables.Get("Local_Addr");
            //}
            //else
            //{
            try
            {

                // System.Net.IPAddress addr;
                // 获得本机局域网IP地址 
                //addr = new System.Net.IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
                //return addr.ToString() + System.Net.Dns.GetHostName();


                string hostName = Dns.GetHostName();
                var hostEntity = Dns.GetHostEntry(hostName);
                var ipAddressList = hostEntity.AddressList;
                var ipAddress = ipAddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                if (ipAddress != null)
                {
                    str = ipAddress.ToString();
                }
                return str;
            }
            catch (Exception) { str = string.Empty; }
            //}
            return str;
        }
    }
}