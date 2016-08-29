using Logging.Server.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging.Server.Site
{
    /// <summary>
    /// StatisticsViewer 的摘要说明
    /// </summary>
    public class StatisticsViewer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            long start = Convert.ToInt64(context.Request["start"]);
            long end = Convert.ToInt64(context.Request["end"]);
            int appId = Convert.ToInt32(context.Request["appId"]);

            //long start_num = Utils.GetTimeStamp(start);

            //long end_num = Utils.GetTimeStamp(end);

            var viewer = LogViewerManager.GetLogViewer();

            var s = viewer.GetStatistics(start, end, appId);
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");//允许跨域请求
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(s));
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