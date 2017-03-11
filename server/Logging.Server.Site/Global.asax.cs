using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Logging.Server.Site
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            // 在应用程序启动时运行的代码
            BackgroundTask.InitTasks();
        }

        private void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
        }
    }
}