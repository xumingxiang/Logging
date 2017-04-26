using System.Web.Mvc;

namespace Logging.Server.Web
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("this is logging server !");
        }
    }
}