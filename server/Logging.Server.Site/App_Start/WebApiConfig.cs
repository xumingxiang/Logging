using System.Net.Http.Formatting;
using System.Web.Http;

namespace Logging.Server.Site
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            // Web API 路由
            config.MapHttpAttributeRoutes();
        }
    }
}