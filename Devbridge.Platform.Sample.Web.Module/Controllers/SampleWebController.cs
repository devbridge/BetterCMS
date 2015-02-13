using System.Web.Mvc;

using Devbridge.Platform.Core.Web.Mvc;
using Devbridge.Platform.Sample.Web.Module.Commands.GetModulesList;

namespace Devbridge.Platform.Sample.Web.Module.Controllers
{
    public class SampleWebController : CoreControllerBase
    {
        public ActionResult Index()
        {
            return Content("Hello World From Sample Controller!");
        }
        
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult ModulesList()
        {
            var command = GetCommand<GetModulesListCommand>();
            var model = command.Execute();

            return View(model);
        }
    }
}
