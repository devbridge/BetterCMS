using System.Web.Mvc;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult PageNotFound()
        {
            return View(new RenderPageViewModel());
        }
    }
}
