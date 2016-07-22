using Logging.Server.Alerting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging.Server.Site.alerting
{
    /// <summary>
    /// SetAppErrOpts 的摘要说明
    /// </summary>
    public class SetAppErrOpts : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var interval = Convert.ToInt32(context.Request["interval"]);
            var errorCountLimit = Convert.ToInt32(context.Request["errorCountLimit"]);
            var errorGrowthLimit = Convert.ToInt32(context.Request["errorGrowthLimit"]);
            var emailReceivers = context.Request["emailReceivers"];
            AppErrorthAlerting.SetOptions(interval, errorCountLimit, errorGrowthLimit, emailReceivers);
            context.Response.Write("{'status':1}");
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