using System.Collections.Generic;

namespace Logging.Server.Web
{
    public class LogVM
    {
        public long Start { get; set; }

        public long End { get; set; }

        public long Cursor { get; set; }

        public List<LogEntity> List { get; set; }
    }
}