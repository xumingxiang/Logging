using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging.Server.Site.metrics
{
    /// <summary>
    /// MetricsQuery 的摘要说明
    /// </summary>
    public class MetricsQuery : IHttpHandler
    {
        string InfluxdbConnectionString = System.Configuration.ConfigurationManager.AppSettings["InfluxdbConnectionString"];
        string MetricInfluxdbHost = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbHost"];
        string MetricInfluxdbPort = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbPort"];
        string MetricInfluxdbDBName = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbDBName"];
        string MetricInfluxdbUser = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbUser"];
        string MetricInfluxdbPwd = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbPwd"];


        public void ProcessRequest(HttpContext context)
        {
            InfluxdbConnectionString = $"http://{MetricInfluxdbHost}:{MetricInfluxdbPort}/db/{MetricInfluxdbDBName}/series?u={MetricInfluxdbUser}&p={MetricInfluxdbPwd}";

         
            context.Response.ContentType = "text/plain";

            string queryCmd = context.Request["cmd"];

            string queryUrl = InfluxdbConnectionString + "&q=" + queryCmd;

            string resp = Utils.HttpGet(queryUrl);
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");//允许跨域请求
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