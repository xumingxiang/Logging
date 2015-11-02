using Logging.Server.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging.Server.Site
{
    /// <summary>
    /// GetLogOnOffs 的摘要说明
    /// </summary>
    public class GetLogOnOffs : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var on_offs = LogOnOffManager.GetALLLogOnOff();
            context.Response.ContentType = "text/plain";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(on_offs));
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