using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;

namespace Logging.Server.Metric.Writer
{
    public partial class InfluxdbWriter
    {
        //  private readonly InfluxDbClient influxDbClient;

        private static string host = ConfigurationManager.AppSettings["MetricInfluxdbHost"];
        private static string port = ConfigurationManager.AppSettings["MetricInfluxdbPort"];
        private static string database = ConfigurationManager.AppSettings["MetricInfluxdbDBName"];
        private static string user = ConfigurationManager.AppSettings["MetricInfluxdbUser"];
        private static string pass = ConfigurationManager.AppSettings["MetricInfluxdbPwd"];

        // private static string influxDbConnectString;


        /// <summary>
        /// 描述：将LogMetric写入Influxdb数据库
        /// 作者：徐明祥
        /// 日期：20160411
        /// </summary>
        /// <param name="logs"></param>
        public void WriteAsync(IList<MetricEntity> logs)
        {
            if (logs == null || logs.Count == 0) { return; }
            string ms = GetMetricsString(logs);
            string writeUrl = $"http://{host}:{port}/write?db={database}&u={user}&p={pass}";
           // string writeUrl = $"http://{host}:{port}/write";
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                var result = client.UploadData(new System.Uri(writeUrl), System.Text.Encoding.UTF8.GetBytes(ms));
                var result2 = System.Text.Encoding.UTF8.GetString(result);
            }
        }

        private string GetMetricsString(IList<MetricEntity> metrics)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < metrics.Count; i++)
            {
                var m = metrics[i];
                sb.Append(m.Name);
                if (m.Tags != null)
                {
                    foreach (var t in m.Tags)
                    {
                        sb.Append($",{t.Key}={ t.Value}");
                    }
                }
                sb.Append($" value={ m.Value}");
                if (m.Time > 0)
                {
                    sb.Append($" {m.Time}");
                }
                if (i < metrics.Count - 1)
                {
                    sb.Append("\n");
                }
            }
            return sb.ToString();
        }
    }
}