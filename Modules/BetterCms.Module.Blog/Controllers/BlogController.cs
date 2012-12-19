using System.Web.Mvc;

using BetterCms.Core.Mvc;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Controllers
{
    public class BlogController : CmsControllerBase
    {
        public ActionResult Menu()
        {
            return Content("MENU CONTENT");
        }

        public virtual ActionResult Index()
        {
            return View();
        }       
    }
}
