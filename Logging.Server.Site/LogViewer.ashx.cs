using Logging.Server.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Logging.Server.Site
{
    /// <summary>
    /// LogViewer 的摘要说明
    /// </summary>
    public class LogViewer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            DateTime start = Convert.ToDateTime(context.Request["start"]);
            DateTime end = Convert.ToDateTime(context.Request["end"]);
            int appId = Convert.ToInt32(context.Request["appId"]);

            string level_str = context.Request["level"];
            if (string.IsNullOrWhiteSpace(level_str))
            {
                level_str = "1,2,3,4";
            }
            int[] level = Array.ConvertAll<string, int>(level_str.Split(','), x => Convert.ToInt32(x));

            string title = context.Request["title"];
            string msg = context.Request["msg"];
            string ip = context.Request["ip"];
            string source = context.Request["source"];
            int limit = Convert.ToInt32(context.Request["limit"]);

            var viewer = LogViewerManager.GetLogViewer();

            var result = new LogSearchVM();

            long ipNum = Utils.IPToNumber(ip);

            long start_num = Utils.GetTimeStamp(start);

            long end_num = Utils.GetTimeStamp(end);

            var lst = viewer.GetLogs(start_num, end_num, appId, level, title, msg, source, (int)ipNum, limit);

            result.List = lst;
            result.Start = start;
            result.End = end;
            var last = lst.LastOrDefault();
            if (last != null)
            {
                result.Cursor = Utils.GetDateTime(lst.Last().Time);
            }
            var json_result = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            context.Response.AddHeader("Access-Control-Allow-Origin", "*");//允许跨域请求

            context.Response.Write(json_result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    internal class LogSearchVM
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTime Cursor { get; set; }

        public List<LogEntity> List { get; set; }
    }
}