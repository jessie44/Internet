using System.Web.Mvc;
using Orchard.Themes;

namespace MBMDCMaps.Controllers
{
    [Themed]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Home");
        }
    }
}