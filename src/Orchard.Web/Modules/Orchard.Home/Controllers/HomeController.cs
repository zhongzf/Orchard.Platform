using Orchard.Themes;
using System.Web.Mvc;


namespace Orchard.Home.Controllers
{
    [Themed]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}