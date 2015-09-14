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

            long start = Convert.ToInt64(context.Request["start"]);
            long end = Convert.ToInt64(context.Request["end"]);
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
            string tags_str = context.Request["tags"];
            var viewer = LogViewerManager.GetLogViewer();

            var result = new LogVM();

            long ipNum = Utils.IPToNumber(ip);

            //long start_num = Utils.GetTimeStamp(start);

            //long end_num = Utils.GetTimeStamp(end);

            List<string> tags = new List<string>();
            if (!string.IsNullOrWhiteSpace(tags_str))
            {
                tags = tags_str.Split(',').Distinct().ToList();

            }

            //tags.Add("a=a");

            var lst = viewer.GetLogs(start, end, appId, level, title, msg, source, (int)ipNum, tags, limit);

            result.List = lst;
            result.Start = start;
            result.End = end;
            var last = lst.LastOrDefault();
            if (last != null)
            {
                result.Cursor = lst.Min(x => x.Time);
                long min = result.List.Min(x => x.Time);
                long max = result.List.Max(x => x.Time);

                long first = result.List.FirstOrDefault().Time;
                long lastt = result.List.LastOrDefault().Time;
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

    public class LogVM
    {
        public long Start { get; set; }

        public long End { get; set; }

        public long Cursor { get; set; }

        public List<LogEntity> List { get; set; }
    }
}