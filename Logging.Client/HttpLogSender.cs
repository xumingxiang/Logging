using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;

namespace Logging.Client
{
    /// <summary>
    /// 发送消息
    /// </summary>
    internal class HttpLogSender : LogSender
    {

        //private static readonly Freeway.Logging.ILog _logger = Freeway.Logging.LogManager.GetLogger(typeof(T));
        public override void Send(IList<LogEntity> logEntities)
        {
            //TTransport transport = new Thrift.Transport.THttpHandler("localhost", 9090);
            //TProtocol protocol = new TBinaryProtocol(transport);
            //Calculator.Client client = new Calculator.Client(protocol);

            //var transport = new Thrift.Transport.("http://localhost:99");
            //   var protocol = new Thrift.Protocol(transport);
            //  var client = new UserStorageClient(protocol);
            var loggingServerUrl = System.Configuration.ConfigurationManager.AppSettings["LoggingServerUrl"];


            var uri = new Uri(loggingServerUrl);

            var httpClient = new THttpClient(uri);

            httpClient.ConnectTimeout = SENDER_TIMEOUT;
            var protocol = new TBinaryProtocol(httpClient);
            httpClient.Open();
            var client = new LogTransferService.Client(protocol);

            List<global::LogEntity> _logEntities = new List<global::LogEntity>();
            foreach (var item in logEntities)
            {
                global::LogEntity _log = new global::LogEntity();
                _log.IP = item.IP;
                _log.Level = (sbyte)item.Level;
                _log.Message = item.Message;
                _log.Tags = item.Tags;
                _log.Title = item.Title;
                _logEntities.Add(_log);
            }
            client.Log(_logEntities);

            httpClient.Close();
        }



    }
}
