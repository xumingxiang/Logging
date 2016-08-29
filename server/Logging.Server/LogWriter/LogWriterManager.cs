using System;
using System.Configuration;

namespace Logging.Server.Writer
{
    public sealed class LogWriterManager
    {

      static  string LoggingStorage = ConfigurationManager.AppSettings["LoggingStorage"];

        private LogWriterManager()
        { }

        public static ILogWriter GetLogWriter()
        {
        
            if (LoggingStorage.Equals("mongodb", StringComparison.OrdinalIgnoreCase))
            {
                return new MongoDbWriter();
            }
            else if (LoggingStorage.Equals("hbase", StringComparison.OrdinalIgnoreCase))
            {
                return new HBaseWriter();
            }
            else
            {
                return new MongoDbWriter();
            }
        }
    }
}