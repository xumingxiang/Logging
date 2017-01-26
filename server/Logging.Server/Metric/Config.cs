using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.Metric
{
    public class Config
    {
        public static readonly string MetricInfluxdbHost = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbHost"];
        public static readonly string MetricInfluxdbPort = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbPort"];
        public static readonly string MetricInfluxdbDBName = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbDBName"];
        public static readonly string MetricInfluxdbUser = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbUser"];
        public static readonly string MetricInfluxdbPwd = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbPwd"];
        public static readonly string MetricInfluxdbVer = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbVer"];

    }
}
