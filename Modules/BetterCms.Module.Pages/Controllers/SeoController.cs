using System.Web.Mvc;

using BetterCms.Module.Pages.Command.Page.GetPageSeo;
using BetterCms.Module.Pages.Command.Page.SavePageSeo;
using BetterCms.Module.Pages.ViewModels.Seo;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Defines logic to handle page SEO information.
    /// </summary>
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

            return View(model);
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
                success = GetCommand<SavePageSeoCommand>().ExecuteCommand(model);
            }

            return Json(new WireJson(success));
        }
    }
}
