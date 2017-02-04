using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Logging.Server.Metric.Writer
{
    /// <summary>
    /// influxdb 0.8.8 以上版本使用
    /// </summary>
    public partial class InfluxdbWriter
    {
        //  private readonly InfluxDbClient influxDbClient;

        private static string host = Config.MetricInfluxdbHost;// ConfigurationManager.AppSettings["MetricInfluxdbHost"];
        private static string port = Config.MetricInfluxdbPort;// ConfigurationManager.AppSettings["MetricInfluxdbPort"];
        private static string database = Config.MetricInfluxdbDBName;// ConfigurationManager.AppSettings["MetricInfluxdbDBName"];
        private static string user = Config.MetricInfluxdbUser;// ConfigurationManager.AppSettings["MetricInfluxdbUser"];
        private static string pass = Config.MetricInfluxdbPwd;// ConfigurationManager.AppSettings["MetricInfluxdbPwd"];

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
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.UploadStringAsync(new System.Uri(writeUrl), ms);
            }
        }

        /// <summary>
        /// 描述：将LogMetric写入Influxdb数据库
        /// 作者：徐明祥
        /// 日期：20160411
        /// </summary>
        /// <param name="logs"></param>
        public void Write(IList<MetricEntity> logs)
        {
            if (logs == null || logs.Count == 0) { return; }
            //foreach (var item in logs)
            //{
            string ms = GetMetricsString(logs);
            string writeUrl = $"http://{host}:{port}/write?db={database}&u={user}&p={pass}";
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");
                client.UploadString(new System.Uri(writeUrl), ms);
            }
            //}
        }

        //private string GetMetricsString(IList<MetricEntity> metrics)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < metrics.Count; i++)
        //    {
        //        var m = metrics[i];
        //        sb.Append(m.Name);
        //        if (m.Tags != null)
        //        {
        //            foreach (var t in m.Tags)
        //            {
        //                sb.Append(",");
        //                sb.Append(t.Key + "=" + t.Value);
        //            }
        //        }
        //        sb.Append(" ");
        //        sb.Append("value=" + m.Value);
        //        if (i < metrics.Count - 1)
        //        {
        //            sb.Append("\n");
        //        }
        //    }
        //    return sb.ToString();
        //}

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
                    sb.Append($" {m.Time * 1000000000 }");
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