using System.Web.Mvc;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Controllers
{
    public class BlogController : CmsControllerBase
    {
        public virtual ActionResult Index()
        {
            return View();
        }       
    }
}
