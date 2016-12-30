namespace Logging.Client.LogSender
{
    internal sealed class LogSenderManager
    {
        private LogSenderManager()
        {
        }

        public static LogSenderBase GetLogSender()
        {
            return new THttpLogSender();
        }
    }
}