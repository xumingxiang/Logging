using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Logging.Server.Web.Startup))]

namespace Logging.Server.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // ConfigureAuth(app);
        }
    }
}