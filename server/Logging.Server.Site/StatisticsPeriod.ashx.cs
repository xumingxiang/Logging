using Logging.Server.Viewer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging.Server.Site
{
    /// <summary>
    /// StatisticsViewer 的摘要说明
    /// </summary>
    public class StatisticsPeriod : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            long period = Convert.ToInt64(context.Request["period"]);
            //long end = Convert.ToInt64(context.Request["end"]);
            int appId = Convert.ToInt32(context.Request["appId"]);

            long start_num = Utils.GetTimeStamp(DateTime.Now.AddMinutes(-period));

            long end_num = Utils.GetTimeStamp(DateTime.Now);

            var viewer = LogViewerManager.GetLogViewer();

            var s = viewer.GetStatistics(start_num, end_num, appId);

            JObject obj = new JObject();
            obj.Add("Data", JToken.FromObject(s));

            context.Response.AddHeader("Access-Control-Allow-Origin", "*");//允许跨域请求
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
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