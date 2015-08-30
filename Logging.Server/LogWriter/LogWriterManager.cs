namespace Logging.Server.Writer
{
    internal sealed class LogWriterManager
    {
        private LogWriterManager()
        { }

        public static ILogWriter GetLogWriter()
        {
            return new MongoDbWriter();
        }
    }
}