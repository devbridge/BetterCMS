using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.Page.GetPageSeo;
using BetterCms.Module.Pages.Command.Page.SavePageSeo;
using BetterCms.Module.Pages.ViewModels.Seo;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Defines logic to handle page SEO information.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class SeoController : CmsControllerBase
    {
        /// <summary>
        /// Creates edit SEO modal dialog for given page.
        /// </summary>
        /// <param name="pageId">A page id.</param>
        /// <returns>
        /// ViewResult to render edit SEO modal dialog.
        /// </returns>
        [HttpGet]        
        public ActionResult EditSeo(string pageId)
        {
            EditSeoViewModel model = GetCommand<GetPageSeoCommand>().ExecuteCommand(pageId.ToGuidOrDefault());
            var view = RenderView("EditSeo", model);
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates and saves page SEO information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json with result status.</returns>
        [HttpPost]
        public ActionResult EditSeo(EditSeoViewModel model)
        {
            bool success = false;            
            if (ModelState.IsValid)
            {
                model = GetCommand<SavePageSeoCommand>().ExecuteCommand(model);
                success = model != null;
            }

            return WireJson(success, model);
        }
    }
}
