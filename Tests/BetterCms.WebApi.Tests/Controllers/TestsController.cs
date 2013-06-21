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
                                                TestsJsUrl = Url.Content("~/Scripts/pages.webapi.tests.js")
                                            });
        }
    }
}
