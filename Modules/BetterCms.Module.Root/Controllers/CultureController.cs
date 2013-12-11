using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.Root.Commands.Culture.DeleteCulture;
using BetterCms.Module.Root.Commands.Culture.GetCultureList;
using BetterCms.Module.Root.Commands.Culture.SaveCulture;
using BetterCms.Module.Root.Commands.Culture.SuggestCultures;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.Autocomplete;
using BetterCms.Module.Root.ViewModels.Cultures;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Cultures / multilanguage controller.
    /// </summary>
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class CultureController : CmsControllerBase
    {
        /// <summary>
        /// Lists the template for displaying cultures list.
        /// </summary>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult ListTemplate()
        {
            var view = RenderView("List", null);
            var request = new SearchableGridOptions();
            request.SetDefaultPaging();

            var cultures = GetCommand<GetCultureListCommand>().ExecuteCommand(request);

            return ComboWireJson(cultures != null, view, cultures, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lists the cultures.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult CulturesList(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetCultureListCommand>().ExecuteCommand(request);
            return WireJson(model != null, model);
        }

        /// <summary>
        /// Saves the culture.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SaveCulture(CultureViewModel model)
        {
            var success = false;
            CultureViewModel response = null;
            if (ModelState.IsValid)
            {
                response = GetCommand<SaveCultureCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(RootGlobalization.CreateCulture_CreatedSuccessfully_Message);
                    }

                    success = true;
                }
            }

            return WireJson(success, response);
        }

        /// <summary>
        /// Deletes the culture.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult DeleteCulture(string id, string version)
        {
            var request = new CultureViewModel { Id = id.ToGuidOrDefault(), Version = version.ToIntOrDefault() };
            var success = GetCommand<DeleteCultureCommand>().ExecuteCommand(request);
            if (success)
            {
                if (!request.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(RootGlobalization.DeleteCulture_DeletedSuccessfully_Message);
                }
            }

            return WireJson(success);
        }

        /// <summary>
        /// Suggests the cultures.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Suggested cultures list</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SuggestCultures(SuggestionViewModel model)
        {
            var suggestedCultures = GetCommand<SuggestCulturesCommand>().ExecuteCommand(model);

            return Json(new { suggestions = suggestedCultures });
        }
    }
}