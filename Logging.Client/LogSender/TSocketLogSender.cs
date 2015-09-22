using Logging.ThriftContract;
using System;
using System.Collections.Generic;
using System.Configuration;
using Thrift.Protocol;
using Thrift.Transport;

namespace Logging.Client
{
    /// <summary>
    /// Thrift Socket协议发送消息
    /// </summary>
    internal class TSocketLogSender : LogSenderBase
    {
        public override void Send(IList<LogEntity> logEntities)
        {
            if (logEntities == null || logEntities.Count <= 0) { return; }
            var _logEntities = new List<TLogEntity>();
            foreach (var item in logEntities)
            {
                var _log = new TLogEntity();
                _log.Level = (sbyte)item.Level;
                _log.Message = item.Message;
                _log.Tags = item.Tags;
                _log.Title = item.Title;
                _log.Source = item.Source;
                _log.Thread = item.Thread;
                _log.Time = item.Time;
                _logEntities.Add(_log);
            }
            TLogPackage logPackage = new TLogPackage();
            logPackage.AppId = Settings.AppId;
            logPackage.IP = ServerIPNum;
            logPackage.Items = _logEntities;

            var socket = new TSocket("localhost", 9813);
            socket.Timeout = SENDER_TIMEOUT;
            var transport = new TFramedTransport(socket);
            var protocol = new TCompactProtocol(transport);
            var client = new LogTransferService.Client(protocol);
            transport.Open();
            client.Log(logPackage);
            transport.Close();
           
        }
    }
}