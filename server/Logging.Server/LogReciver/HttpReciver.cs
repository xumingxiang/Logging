using Logging.ThriftContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Logging.Server.Reciver
{
    public class HttpReciver : THttpHandler
    {
        public HttpReciver()
            : base(new LogTransferService.Processor(new LogReciverBase()))
        {
        }
    }
}
