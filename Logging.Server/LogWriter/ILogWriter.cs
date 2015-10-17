using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.Writer
{
   public interface ILogWriter
    {
         void Write(IList<LogEntity> logs);

        void SetLogOnOff(LogOnOff on_off);
    }
}
