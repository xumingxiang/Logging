using Logging.ThriftContract;
using System;
using System.Collections.Generic;
using System.Configuration;
using Thrift.Protocol;
using Thrift.Transport;

namespace Logging.Client
{
    /// <summary>
    /// Thrift Http协议发送消息
    /// </summary>
    internal class THttpLogSender : LogSenderBase
    {
        private readonly static string loggingServerHost = Settings.LoggingServerHost;
        private readonly static Uri uri = new Uri(loggingServerHost + "/Reciver.ashx");

        public override void Send(IList<ILogEntity> logEntities)
        {
            if (logEntities == null || logEntities.Count <= 0) { return; }

            TLogPackage logPackage = this.CreateLogPackage(logEntities);

            var httpClient = new THttpClient(uri);
            httpClient.ConnectTimeout = SENDER_TIMEOUT;
            //var protocol = new TBinaryProtocol(httpClient);
            var protocol = new TCompactProtocol(httpClient);
            httpClient.Open();
            var client = new LogTransferService.Client(protocol);
            client.Log(logPackage);
            httpClient.Close();
        }
    }
}