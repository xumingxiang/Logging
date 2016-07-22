using Logging.Server.Alerting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging.Server.Site.alerting
{
    /// <summary>
    /// GetAppErrOpts 的摘要说明
    /// </summary>
    public class GetAppErrOpts : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var opts = AppErrorthAlerting.GetOptions();
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(opts));
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