using Logging.Server.Metric.Processor;
using Logging.Server.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Logging.Server.Site
{
    /// <summary>
    /// GetLogOnOff 的摘要说明
    /// </summary>
    public class GetLogOnOff : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int appId = Convert.ToInt32(context.Request["appId"]);

            string resp = string.Empty;

            Parallel.Invoke(() =>
            {
                var on_off = LogViewerManager.GetLogViewer().GetLogOnOff(appId);
                if (on_off != null)
                {
                    resp += on_off.Debug + "," + on_off.Info + "," + on_off.Warm + "," + on_off.Error;
                }
            }, () =>
            {
                #region 计数
                MetricEntity metric = new MetricEntity();
                metric.Name = "logging_client_getLogOnOff_count";
                metric.Time = Utils.GetTimeStamp(DateTime.Now) / 1000;
                metric.Value = 1;
                metric.Tags = new Dictionary<string, string>();
                metric.Tags.Add("AppId", appId.ToString());
                List<MetricEntity> metrics = new List<MetricEntity>();
                metrics.Add(metric);
                var metricProcessor = MetricProcessorManager.GetMetricProcessor();
                metricProcessor.Process(metrics);
                #endregion
            });
            context.Response.Write(resp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}