using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logging.Server.Writer;

namespace Logging.Server.Site
{
    /// <summary>
    /// SetLogOnOff 的摘要说明
    /// </summary>
    public class SetLogOnOff : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int appId = Convert.ToInt32(context.Request["appId"]);
            int debug = Convert.ToInt32(context.Request["debug"]);
            int info = Convert.ToInt32(context.Request["info"]);
            int warm = Convert.ToInt32(context.Request["warm"]);
            int error = Convert.ToInt32(context.Request["error"]);

            LogOnOff on_off = new LogOnOff();
            on_off.AppId = appId;
            on_off.Debug = (byte)debug;
            on_off.Info = (byte)info;
            on_off.Warm = (byte)warm;
            on_off.Error = (byte)error;

            LogWriterManager.GetLogWriter().SetLogOnOff(on_off);


            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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