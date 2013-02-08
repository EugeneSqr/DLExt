using System.Web.Mvc;

namespace DLExt.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = Resources.CommonResources.HomeHeader;
            
            return View();
        }
    }
}
