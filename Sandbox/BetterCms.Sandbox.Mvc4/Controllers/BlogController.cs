using System.Web.Mvc;

using BetterCms.Sandbox.Mvc4.Models;

namespace BetterCms.Sandbox.Mvc4.Controllers
{
    public class BlogController : Controller
    {
        public ActionResult Index()
        {
            var model = new BlogTestViewModel { Message = "Test Blog View Model Message" };

            return View(model);
        }
    }
}