using System;
using System.Web.Mvc;

using BetterCms.Module.Blog.Commands.GetTemplatesList;
using BetterCms.Module.Blog.Commands.SaveDefaultTemplate;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Controllers
{
    public class OptionController : CmsControllerBase
    {
        public virtual ActionResult Templates()
        {
            var response = GetCommand<GetTemplatesCommand>().ExecuteCommand(true);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult SaveDefaultTemplate(Guid id)
        {
            var response = GetCommand<SaveDefaultTemplateCommand>().ExecuteCommand(id);

            return WireJson(response);
        }
    }
}