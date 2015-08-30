namespace Logging.Server.Processor
{
    internal sealed class LogProcessorManager
    {
        private LogProcessorManager()
        { }

        public static ILogProcessor GetLogProcessor()
        {
            return new DefaultLogProcessor();
        }
    }
}