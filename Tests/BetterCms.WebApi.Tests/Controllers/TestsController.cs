using System.Web.Mvc;

using BetterCms.WebApi.Tests.Models;

namespace BetterCms.WebApi.Tests.Controllers
{
    public class TestsController : Controller
    {
        public ActionResult PagesWebApiTests()
        {
            return View("TestsResults", new TestsRunnerViewModel
                                            {
                                                Title = "Pages WebAPI Tests",
                                                TestsJsUrl = Url.Content("~/Scripts/Pages/pages.webapi.tests.js")
                                            });
        }

        public ActionResult RootTags()
        {
            return View("TestsResults", new TestsRunnerViewModel
            {
                Title = "Root.Tags WebAPI Tests",
                TestsJsUrl = Url.Content("~/Scripts/Root/tags.webapi.tests.js")
            });
        }
    }
}
