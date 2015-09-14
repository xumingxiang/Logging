using Logging.ThriftContract;
using System;
using System.Collections.Generic;
using System.Configuration;
using Thrift.Protocol;
using Thrift.Transport;

namespace Logging.Client
{
    /// <summary>
    /// 发送消息
    /// </summary>
    internal class HttpLogSender : LogSenderBase
    {
        public override void Send(IList<LogEntity> logEntities)
        {
            if (logEntities == null || logEntities.Count <= 0) { return; }

            string loggingServerHost = ConfigurationManager.AppSettings["LoggingServerHost"] ?? Settings.LoggingServerHost;

            var uri = new Uri(loggingServerHost + "/Reciver.ashx");
            var httpClient = new THttpClient(uri);
            httpClient.ConnectTimeout = SENDER_TIMEOUT;           
            var protocol = new TBinaryProtocol(httpClient);
           
            httpClient.Open();
            var client = new LogTransferService.Client(protocol);
           

            var _logEntities = new List<TLogEntity>();
            foreach (var item in logEntities)
            {
                var _log = new TLogEntity();
                _log.IP = item.IP;
                _log.Level = (sbyte)item.Level;
                _log.Message = item.Message;
                _log.Tags = item.Tags;
                _log.Title = item.Title;
                _log.Source = item.Source;
                _log.Thread = item.Thread;
                _log.Time = item.Time;
                _log.AppId = item.AppId;
                _logEntities.Add(_log);
            }
            client.Log(_logEntities);
          
            httpClient.Close();
        }
    }
}