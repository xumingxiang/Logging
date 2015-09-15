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
            throw new NotImplementedException();
        }
    }
}