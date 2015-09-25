using Logging.Server.Metric.Processor;
using Logging.Server.Processor;
using Logging.ThriftContract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Logging.Server.Reciver
{
    public class LogReciverBase : LogTransferService.Iface
    {
        private static BlockingActionQueue<TLogPackage> queue;

        private static int server_appId = Convert.ToInt32(ConfigurationManager.AppSettings["AppId"]);

        public LogReciverBase()
        {
        }

        static LogReciverBase()
        {
            int processTaskNum = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessTaskNum"]);
            int blockingQueueLength = Convert.ToInt32(ConfigurationManager.AppSettings["BlockingQueueLength"]);

            queue = new BlockingActionQueue<TLogPackage>(processTaskNum, (logPackage) =>
            {
                ProcessLog(logPackage);
                ProcessMetric(logPackage);
            }, blockingQueueLength);
        }

        private static void ProcessLog(TLogPackage logPackage)
        {
            if (logPackage.LogItems == null || logPackage.LogItems.Count == 0) { return; }

            List<TLogEntity> logEntities = logPackage.LogItems;
            var _logEntities = new List<LogEntity>();
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
                _log.IP = logPackage.IP;
                _log.Level = (LogLevel)item.Level;
                _log.Message = item.Message;
                _log.Tags = tags;
                _log.Title = item.Title;
                _log.Source = item.Source;
                _log.Thread = item.Thread;
                _log.Time = item.Time;
                _log.AppId = logPackage.AppId;
                _logEntities.Add(_log);
            }

            var processor = LogProcessorManager.GetLogProcessor();
            processor.Process(_logEntities);
        }

        private static void ProcessMetric(TLogPackage logPackage)
        {
            if (logPackage.MetricItems == null || logPackage.MetricItems.Count == 0) { return; }
            string appId = logPackage.AppId.ToString();
            string ip = Utils.NumberToIP(logPackage.IP);
            List<MetricEntity> metrics = new List<MetricEntity>();
            foreach (var item in logPackage.MetricItems)
            {
                MetricEntity metric = new MetricEntity();
                metric.Name = item.Name;
                metric.Time = item.Time;
                metric.Tags = item.Tags;
                metric.Value = item.Value;
                if (metric.Tags == null)
                {
                    metric.Tags = new Dictionary<string, string>();
                }
                if (!metric.Tags.ContainsKey("AppId"))
                {
                    metric.Tags.Add("AppId", appId);
                }
                if (!metric.Tags.ContainsKey("IP"))
                {
                    metric.Tags.Add("IP", ip);
                }
                metrics.Add(metric);
            }
            var metricProcessor = MetricProcessorManager.GetMetricProcessor();
            metricProcessor.Process(metrics);
        }

        /// <summary>
        /// Thrift实现方法
        /// </summary>
        /// <param name="logPackage"></param>
        public void Log(TLogPackage logPackage)
        {
            int over_count = queue.Enqueue(logPackage);

            #region 溢出处理

            if (over_count > 0)
            {
                string msg = "Logging_Server_Queue溢出数量:" + over_count + " 。 建议增加 BlockingQueueLength 配置值";
                var over_log_tags = new List<string> { "_title_=Logging_Server_Over" };

                LogEntity over_log = new LogEntity();
                over_log.IP = ServerIPNum;
                over_log.Level = LogLevel.Error;
                over_log.Message = msg;
                over_log.Tags = over_log_tags;
                over_log.Title = "Logging_Server_Over";
                over_log.Source = "Logging.Server.LogReciver";
                over_log.Thread = Thread.CurrentThread.ManagedThreadId;
                over_log.Time = Utils.GetTimeStamp(DateTime.Now);
                over_log.AppId = server_appId;

                List<LogEntity> over_logs = new List<LogEntity>();
                //over_logs.Add(over_log);
                //ProcessLog(over_logs);

                var processor = LogProcessorManager.GetLogProcessor();
                processor.Process(over_logs);
            }

            #endregion 溢出处理
        }

        #region 私有成员

        private static long serverIPNum;

        private static long ServerIPNum
        {
            get
            {
                if (serverIPNum <= 0)
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
            try
            {
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
            return str;
        }


        #endregion 私有成员
    }
}