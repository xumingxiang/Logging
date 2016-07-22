using Logging.Server.Metric.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging.Server.Site.metrics
{
    /// <summary>
    /// Point 的摘要说明
    /// </summary>
    public class Point : IHttpHandler
    {

        string InfluxdbConnectionString = System.Configuration.ConfigurationManager.AppSettings["InfluxdbConnectionString"];
        string MetricInfluxdbHost = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbHost"];
        string MetricInfluxdbPort = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbPort"];
        string MetricInfluxdbDBName = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbDBName"];
        string MetricInfluxdbUser = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbUser"];
        string MetricInfluxdbPwd = System.Configuration.ConfigurationManager.AppSettings["MetricInfluxdbPwd"];


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";




            try
            {
                context.Response.ContentType = "text/plain";

                string name = context.Request["name"];
                double value = Convert.ToDouble(context.Request["value"]);
                string tags_str = context.Request["tags"];

                var tags = new Dictionary<string, string>();

                List<MetricEntity> lst = new List<MetricEntity>();
                MetricEntity m = new MetricEntity();
                m.Name = name;
                m.Value = value;
                if (!string.IsNullOrWhiteSpace(tags_str))
                {

                    var arr_tags = tags_str.Split('&');
                    for (int i = 0; i < arr_tags.Length; i++)
                    {
                        string[] tag = arr_tags[i].Split('=');
                        string tag_key = tag[0];
                        string tag_value = tag[1];
                        if (!string.IsNullOrWhiteSpace(tag_key))
                        {
                            tags[tag_key] = tag_value;
                        }
                    }
                }
                m.Tags = tags;
                m.Time = Utils.GetUnixTime(DateTime.Now) * 1000000000;
                lst.Add(m);
                InfluxdbWriter w = new InfluxdbWriter();
                w.WriteAsync(lst);




                context.Response.AddHeader("Access-Control-Allow-Origin", "*");//允许跨域请求
                                                                               // context.Response.AddHeader("Access-Control-Allow-Credentials", "true");//允许跨域操作cookie

                context.Response.Write("point success :" + context.Request.RawUrl);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex);
            }
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