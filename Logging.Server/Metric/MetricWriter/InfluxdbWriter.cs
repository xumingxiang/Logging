using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxDb.Models;
using System.Collections.Generic;
using System.Configuration;

namespace Logging.Server.Metric.Writer
{
    public partial class InfluxdbWriter
    {
        private readonly InfluxDbClient influxDbClient;

        private static string host = ConfigurationManager.AppSettings["MetricInfluxdbHost"];
        private static string port = ConfigurationManager.AppSettings["MetricInfluxdbPort"];
        private static string database = ConfigurationManager.AppSettings["MetricInfluxdbDBName"];
        private static string user = ConfigurationManager.AppSettings["MetricInfluxdbUser"];
        private static string pass = ConfigurationManager.AppSettings["MetricInfluxdbPwd"];

        public InfluxdbWriter()
        {
            this.influxDbClient = new InfluxDbClient($"http://{host}:{port}/", user, pass, InfluxDbVersion.Latest);
        }

        /// <summary>
        /// 描述：将LogMetric写入Influxdb数据库
        /// 作者：徐明祥
        /// 日期：20160411
        /// </summary>
        /// <param name="logs"></param>
        public void WriteAsync(IList<MetricEntity> logs)
        {
            if (logs == null || logs.Count == 0) { return; }
            List<Point> points = new List<Point>();
            foreach (var item in logs)
            {
                var pointToWrite = new Point();
                pointToWrite.Name = item.Name;
                pointToWrite.Tags = new Dictionary<string, object>();
                if (item.Tags != null)
                {
                    foreach (var tag in item.Tags)
                    {
                        pointToWrite.Tags.Add(tag.Key, tag.Value);
                    }
                }
                pointToWrite.Fields = new Dictionary<string, object>();
                pointToWrite.Fields.Add("value", item.Value);
                if (item.Time > 0)
                {
                    pointToWrite.Timestamp = Utils.GetDateTimeFromUnix(item.Time).ToUniversalTime();
                }
                points.Add(pointToWrite);
            }

            influxDbClient.Client.WriteAsync(database, points);
        }
    }
}