using System;
using System.Configuration;

namespace Logging.Server.Viewer
{
    public sealed class LogViewerManager
    {
        private LogViewerManager()
        { }

        public static ILogViewer GetLogViewer()
        {
            string LoggingStorage = ConfigurationManager.AppSettings["LoggingStorage"];

            if (LoggingStorage.Equals("mongodb", StringComparison.OrdinalIgnoreCase))
            {
                return new MongoDbViewer();
            }
            else if (LoggingStorage.Equals("hbase", StringComparison.OrdinalIgnoreCase))
            {
                return new HBaseViewer();
            }
            else
            {
                return new MongoDbViewer();
            }
        }
    }
}