using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Server
{
    /// <summary>
    /// 处理队列元素(日志)的处理器
    /// </summary>
    internal class LogProcessor : ILogProcessor
    {
        public void Process(IList<LogEntity> LogEntities)
        {
            throw new NotImplementedException();
        }
    }
}
