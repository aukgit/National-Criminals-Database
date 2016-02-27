using System.Web.Mvc;

namespace NCD.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}