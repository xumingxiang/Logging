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

        static string loggingServerHost = ConfigurationManager.AppSettings["LoggingServerHost"] ?? Settings.LoggingServerHost;
        static Uri uri = new Uri(loggingServerHost + "/Reciver.ashx");

        public override void Send(IList<LogEntity> logEntities)
        {
            Logger.Log("6：Send logEntities");


            if (logEntities == null || logEntities.Count <= 0) { return; }


            Logger.Log("7：" + logEntities.Count);
            THttpClient httpClient = null
                ;
            try
            {
                httpClient = new THttpClient(uri);
                httpClient.ConnectTimeout = SENDER_TIMEOUT;
                //var protocol = new TBinaryProtocol(httpClient);
                var protocol = new TCompactProtocol(httpClient);
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




                Logger.Log("8：client.Log");

                client.Log(_logEntities);
            }
            catch (Exception ex)
            {
                Logger.Log("9：client.Log Exception");
                Logger.Log(ex.ToString());

                Logger.Log(ex.StackTrace);
            }

            httpClient.Close();

        }
    }
}