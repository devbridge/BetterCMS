using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.Redirect.DeleteRedirect;
using BetterCms.Module.Pages.Command.Redirect.GetRedirectsList;
using BetterCms.Module.Pages.Command.Redirect.SaveRedirect;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class RedirectController : CmsControllerBase
    {
        /// <summary>
        /// Renders a redirects list for the site settings dialog.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered redirects list.
        /// </returns>
        public ActionResult Redirects(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetRedirectsListCommand>().ExecuteCommand(request);
            return View(model);
        }

        /// <summary>
        /// Deletes redirect.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        public ActionResult DeleteRedirect(SiteSettingRedirectViewModel model)
        {
            var success = GetCommand<DeleteRedirectCommand>().ExecuteCommand(model);
            if (success)
            {
                Messages.AddSuccess(PagesGlobalization.DeleteRedirect_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }

        /// <summary>
        /// Saves redirect.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json with result  status.</returns>
        [HttpPost]
        public ActionResult SaveRedirect(SiteSettingRedirectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveRedirectCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(PagesGlobalization.CreateRedirect_CreatedSuccessfully_Message);
                    }
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }
    }
}
