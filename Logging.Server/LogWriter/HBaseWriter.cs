using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.Writer
{
    internal class HBaseWriter : ILogWriter
    {
        public void Write(IList<LogEntity> logs)
        {
            throw new NotImplementedException();
        }
    }
}
