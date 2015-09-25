using Logging.Client.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Logging.Client.TestSite
{
    public class Global : System.Web.HttpApplication
    {

        HttpUnhandledExceptionAggregater AppErrorAggregater = new Widgets.HttpUnhandledExceptionAggregater(100, 5);

        protected void Application_Start(object sender, EventArgs e)
        {
            Logging.Client.Settings.LoggingBlockElapsed = 1000 * 10;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {

        }


        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }



        protected void Application_Error(object sender, EventArgs e)
        {
            AppErrorAggregater.Aggregate();
        }

        protected void Global_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}