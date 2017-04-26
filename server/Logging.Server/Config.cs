namespace Logging.Server
{
    public sealed class Config
    {
        public static readonly string LoggingStorageType = System.Configuration.ConfigurationManager.AppSettings["LoggingStorageType"];
        public static readonly string BlockingQueueLength = System.Configuration.ConfigurationManager.AppSettings["BlockingQueueLength"];
        public static readonly string ProcessTaskNum = System.Configuration.ConfigurationManager.AppSettings["ProcessTaskNum"];
        public static readonly string AppId = System.Configuration.ConfigurationManager.AppSettings["AppId"];
        public static readonly string Listeners = System.Configuration.ConfigurationManager.AppSettings["Listeners"];
        public static readonly string MongodbHost = System.Configuration.ConfigurationManager.AppSettings["MongodbHost"];
        public static readonly string LogMongodbDBName = System.Configuration.ConfigurationManager.AppSettings["LogMongodbDBName"];

        public static readonly string MetricInfluxdbHost = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbHost"];
        public static readonly string MetricInfluxdbPort = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbPort"];
        public static readonly string MetricInfluxdbDBName = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbDBName"];
        public static readonly string MetricInfluxdbUser = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbUser"];
        public static readonly string MetricInfluxdbPwd = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbPwd"];
        public static readonly string MetricInfluxdbVer = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbVer"];
    }
}