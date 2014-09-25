using System.Web.Mvc;

using BetterCms.Sandbox.Mvc4.Models;

using httpContext = System.Web.HttpContext;

namespace BetterCms.Sandbox.Mvc4.Controllers
{
    public class WidgetsController : Controller
    {
        public ActionResult TestPostAndGet(TestPostAndGetViewModel model)
        {
            return View(model);
        }
    }
}