namespace Logging.Client
{
    internal class SimpleLogger : BaseLogger, ILog
    {
        public SimpleLogger(string source)
            : base(source)
        {
        }
    }
}