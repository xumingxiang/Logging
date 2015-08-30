using Logging.Server.Writer;
using System.Collections.Generic;

namespace Logging.Server.Processor
{
    /// <summary>
    /// 处理队列元素(日志)的处理器
    /// </summary>
    internal class DefaultLogProcessor : ILogProcessor
    {
        public void Process(IList<LogEntity> logs)
        {
            var logWriter = LogWriterManager.GetLogWriter();

            logWriter.Write(logs);
        }
    }
}