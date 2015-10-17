using Logging.Server.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging.Server.Site
{
    /// <summary>
    /// GetLogOnOff 的摘要说明
    /// </summary>
    public class GetLogOnOff : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int appId = Convert.ToInt32(context.Request["appId"]);
            var on_off = LogViewerManager.GetLogViewer().GetLogOnOff(appId);

            string resp = string.Empty;
            if (on_off != null)
            {
                resp += on_off.Debug + "," + on_off.Info + "," + on_off.Warm + "," + on_off.Error;
            }

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