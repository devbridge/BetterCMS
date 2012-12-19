using System.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}
