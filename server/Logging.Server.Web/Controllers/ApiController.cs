using Logging.Server.Alerting;
using Logging.Server.Metric.Writer;
using Logging.Server.Reciver;
using Logging.Server.Viewer;
using Logging.ThriftContract;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Logging.Server.Web
{
    [RoutePrefix("api")]
    public class ApiController : System.Web.Http.ApiController
    {
        [HttpGet, HttpPost, Route("GetLogOnOff")]
        public dynamic GetLogOnOff(int appId = 0)
        {
            string resp = string.Empty;

            var on_off = LogViewerManager.GetLogViewer().GetLogOnOff(appId);
            if (on_off != null)
            {
                resp += on_off.Debug + "," + on_off.Info + "," + on_off.Warn + "," + on_off.Error;
            }

            #region 计数

            TMetricEntity metric = new TMetricEntity();
            metric.Name = "logging_client_getLogOnOff_count";
            metric.Time = Utils.GetTimeStamp(DateTime.Now) / 10000000;
            metric.Value = 1;
            metric.Tags = new Dictionary<string, string>();
            metric.Tags.Add("AppId", appId.ToString());
            List<TMetricEntity> metrics = new List<TMetricEntity>();
            metrics.Add(metric);

            TLogPackage logPackage = new TLogPackage();
            logPackage.AppId = appId;
            logPackage.IP = 0;
            logPackage.MetricItems = metrics;

            LogReciverBase LogReciver = new LogReciverBase();

            LogReciver.Log(logPackage);

            #endregion 计数

            return resp;
        }

        [HttpGet, HttpPost, Route("GetLogOnOffs")]
        public dynamic GetLogOnOffs()
        {
            var on_offs = LogOnOffManager.GetALLLogOnOff();
            return Newtonsoft.Json.JsonConvert.SerializeObject(on_offs);
        }

        [HttpGet, HttpPost, Route("SetLogOnOff")]
        public dynamic SetLogOnOff(int appId, string appName, int debug = 1, int info = 1, int warn = 1, int error = 1)
        {
            LogOnOff on_off = new LogOnOff();
            on_off.AppId = appId;
            on_off.Debug = (byte)debug;
            on_off.Info = (byte)info;
            on_off.Warn = (byte)warn;
            on_off.Error = (byte)error;
            on_off.AppName = appName;

            LogOnOffManager.SetLogOnOff(on_off);

            return 0;
        }

        [HttpGet, HttpPost, Route("LogViewer")]
        public dynamic LogViewer(long start, long end, int appId = 0, string level = "", string title = "", string msg = "", string ip = "", string source = "", int limit = 0, string tags = "")
        {
            //long start = Convert.ToInt64(Request["start"]);
            //long end = Convert.ToInt64(Request.Query["end"]);
            //int appId = Convert.ToInt32(Request.Query["appId"]);

            //string level_str = Request.Query["level"];
            if (string.IsNullOrWhiteSpace(level))
            {
                level = "1,2,3,4";
            }
            List<int> level_arr = new List<int>();

            level.Split(',').ToList().ForEach(x => level_arr.Add(Convert.ToInt32(x)));

            //string title = Request.Query["title"];
            //string msg = Request.Query["msg"];
            //string ip = Request.Query["ip"];
            //string source = Request.Query["source"];
            //int limit = Convert.ToInt32(Request.Query["limit"]);
            //string tags_str = Request.Query["tags"];
            var viewer = LogViewerManager.GetLogViewer();

            var result = new LogVM();

            long ipNum = Utils.IPToNumber(ip);

            List<string> tags_arr = new List<string>();
            if (!string.IsNullOrWhiteSpace(tags))
            {
                tags_arr = tags.Split(',').Distinct().ToList();
            }

            var lst = viewer.GetLogs(start, end, appId, level_arr.ToArray(), title, msg, source, ipNum, tags_arr, limit);

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

            return json_result;
        }

        [HttpGet, HttpPost, Route("StatisticsPeriod")]
        public dynamic StatisticsPeriod(long period, int appId)
        {
            //long period = Convert.ToInt64(Request.Query["period"]);
            //int appId = Convert.ToInt32(Request.Query["appId"]);
            long start_num = Utils.GetTimeStamp(DateTime.Now.AddMinutes(-period));
            long end_num = Utils.GetTimeStamp(DateTime.Now);
            var viewer = LogViewerManager.GetLogViewer();
            var s = viewer.GetStatistics(start_num, end_num, appId);
            JObject obj = new JObject();
            obj.Add("Data", JToken.FromObject(s));
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        [HttpGet, HttpPost, Route("StatisticsViewer")]
        public dynamic StatisticsViewer(long start, long end, int appId = 0)
        {
            //long start = Convert.ToInt64(Request.Query["start"]);
            //long end = Convert.ToInt64(Request.Query["end"]);
            //int appId = Convert.ToInt32(Request.Query["appId"]);
            var viewer = LogViewerManager.GetLogViewer();
            var s = viewer.GetStatistics(start, end, appId);
            return Newtonsoft.Json.JsonConvert.SerializeObject(s);
        }

        [HttpGet, HttpPost, Route("GetAppErrOpts")]
        public dynamic GetAppErrOpts()
        {
            var opts = AppErrorthAlerting.GetOptions();
            return Newtonsoft.Json.JsonConvert.SerializeObject(opts);
        }

        [HttpGet, HttpPost, Route("SetAppErrOpts")]
        public dynamic SetAppErrOpts(int interval = 5, int errorCountLimit = 100, int errorGrowthLimit = 100, string emailReceivers = "")
        {
            //var interval = Convert.ToInt32(Request.Query["interval"]);
            //var errorCountLimit = Convert.ToInt32(Request.Query["errorCountLimit"]);
            //var errorGrowthLimit = Convert.ToInt32(Request.Query["errorGrowthLimit"]);
            //var emailReceivers = Request.Query["emailReceivers"];
            AppErrorthAlerting.SetOptions(interval, errorCountLimit, errorGrowthLimit, emailReceivers);
            return "{'status':1}";
        }

        [HttpGet, HttpPost, Route("MetricsQuery")]
        public dynamic MetricsQuery(string cmd)
        {
            string InfluxdbConnectionString = "";
            if (Config.MetricInfluxdbVer == "0.8")
            {
                InfluxdbConnectionString = $"http://{Config.MetricInfluxdbHost}:{Config.MetricInfluxdbPort}/db/{Config.MetricInfluxdbDBName}/series?u={Config.MetricInfluxdbUser}&p={Config.MetricInfluxdbPwd}";
            }
            else
            {
                InfluxdbConnectionString = $"http://{Config.MetricInfluxdbHost}:{Config.MetricInfluxdbPort}/query?db={Config.MetricInfluxdbDBName}&u={Config.MetricInfluxdbUser}&p={Config.MetricInfluxdbPwd}";
            }

            string queryCmd = cmd;// Request.Query["cmd"];

            string queryUrl = InfluxdbConnectionString + "&q=" + queryCmd;

            string resp = Utils.HttpGet(queryUrl);
            return resp;
        }

        [HttpGet, HttpPost, Route("MetricTags")]
        public dynamic MetricTags(string metricName)
        {
            string InfluxdbConnectionString = $"{Config.MetricInfluxdbHost}:{Config.MetricInfluxdbPort}/db/{Config.MetricInfluxdbDBName}/series?u={Config.MetricInfluxdbUser}&p={Config.MetricInfluxdbPwd}";

            // string metricName = Request.Query["metricName"];

            string queryUrl = $"{InfluxdbConnectionString}&q=select * from {metricName} limit 1";

            string resp = Utils.HttpGet(queryUrl);
            return resp;
        }

        [HttpGet, HttpPost, Route("Metric")]
        public dynamic Metric(string name, double value, string tags)
        {
            //string name = Request.Query["name"];
            //double value = Convert.ToDouble(Request.Query["value"]);
            //string tags_str = Request.Query["tags"];

            var tags_dic = new Dictionary<string, string>();

            List<MetricEntity> lst = new List<MetricEntity>();
            MetricEntity m = new MetricEntity();
            m.Name = name;
            m.Value = value;
            if (!string.IsNullOrWhiteSpace(tags))
            {
                var arr_tags = tags.Split('&');
                for (int i = 0; i < arr_tags.Length; i++)
                {
                    string[] tag = arr_tags[i].Split('=');
                    string tag_key = tag[0];
                    string tag_value = tag[1];
                    if (!string.IsNullOrWhiteSpace(tag_key))
                    {
                        tags_dic[tag_key] = tag_value;
                    }
                }
            }
            m.Tags = tags_dic;
            m.Time = Utils.GetUnixTime(DateTime.Now);
            lst.Add(m);
            var w = new InfluxdbReport();
            w.Write(lst);

            return "point success !";
        }
    }
}