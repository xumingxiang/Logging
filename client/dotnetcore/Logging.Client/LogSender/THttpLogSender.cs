using Logging.ThriftContract;
using System;
using System.Collections.Generic;
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
        private readonly static long TransDataSizeLimit = 1 * 1024 * 1024L;

        public override long Send(IList<ILogEntity> logEntities)
        {
            //  Console.WriteLine(logEntities.Count);

            if (logEntities == null || logEntities.Count <= 0) { return 0; }

            TLogPackage logPackage = this.CreateLogPackage(logEntities);

            var trans = new THttpClient(uri);
            trans.ConnectTimeout = SENDER_TIMEOUT;
            trans.DataSizeLimit = TransDataSizeLimit;
            //var protocol = new TBinaryProtocol(httpClient);
            var protocol = new TCompactProtocol(trans);
            trans.Open();

            var client = new LogTransferService.Client(protocol);
            client.Log(logPackage);

            long data_size = trans.DataSize;

            trans.Close();
            return data_size;
        }
    }
}