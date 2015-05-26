using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thrift.Transport;

namespace Logging.Server.Site
{
    /// <summary>
    /// Recive 的摘要说明
    /// </summary>
    public class Reciver : THttpHandler
    {
        public Reciver()
            : base(new LogTransferService.Processor(new LogReciver()))
        {
        }
    }
}