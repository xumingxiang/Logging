using Logging.ThriftContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Server;
using Thrift.Transport;

namespace Logging.Server.Reciver
{
    public class SocketReciver
    {
        public SocketReciver()
        {
            try
            {
                TServerSocket serverTransport = new TServerSocket(7911, 0, false);
                var processor = new LogTransferService.Processor(new LogReciverBase());
                TServer server = new TSimpleServer(processor, serverTransport);
                Console.WriteLine("Starting server on port 7911 ...");
                server.Serve();
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
