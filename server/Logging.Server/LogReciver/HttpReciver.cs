using Logging.ThriftContract;
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