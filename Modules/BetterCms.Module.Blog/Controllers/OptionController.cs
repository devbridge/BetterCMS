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
            var templates = GetCommand<GetTemplatesCommand>().ExecuteCommand(true);
            var view = RenderView("Templates");
            
            return ComboWireJson(templates != null, view, templates, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult SaveDefaultTemplate(string id)
        {
            var response = GetCommand<SaveDefaultTemplateCommand>().ExecuteCommand(id.ToGuidOrDefault());

            return WireJson(response);
        }
    }
}