using System.Collections.Generic;

namespace Logging.Server.Processor
{
    /// <summary>
    /// 处理队列元素(日志)的处理器
    /// </summary>
    internal interface ILogProcessor
    {
        void Process(IList<LogEntity> logs);
    }
}