using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.Root.Commands.Language.DeleteLanguage;
using BetterCms.Module.Root.Commands.Language.GetLanguageList;
using BetterCms.Module.Root.Commands.Language.SaveLanguage;
using BetterCms.Module.Root.Commands.Language.SuggestLanguages;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.Autocomplete;
using BetterCms.Module.Root.ViewModels.Language;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Multilanguage controller.
    /// </summary>
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    [BcmsAuthorize]
    public class LanguageController : CmsControllerBase
    {
        /// <summary>
        /// Lists the template for displaying languages list.
        /// </summary>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult ListTemplate()
        {
            var view = RenderView("List", null);
            var request = new SearchableGridOptions();
            request.SetDefaultPaging();

            var languages = GetCommand<GetLanguageListCommand>().ExecuteCommand(request);

            return ComboWireJson(languages != null, view, languages, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lists the languages.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult LanguagesList(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetLanguageListCommand>().ExecuteCommand(request);
            return WireJson(model != null, model);
        }

        /// <summary>
        /// Saves the language.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SaveLanguage(LanguageViewModel model)
        {
            var success = false;
            LanguageViewModel response = null;
            if (ModelState.IsValid)
            {
                response = GetCommand<SaveLanguageCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(RootGlobalization.CreateLanguage_CreatedSuccessfully_Message);
                    }

                    success = true;
                }
            }

            return WireJson(success, response);
        }

        /// <summary>
        /// Deletes the language.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult DeleteLanguage(string id, string version)
        {
            var request = new LanguageViewModel { Id = id.ToGuidOrDefault(), Version = version.ToIntOrDefault() };
            var success = GetCommand<DeleteLanguageCommand>().ExecuteCommand(request);
            if (success)
            {
                if (!request.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(RootGlobalization.DeleteLanguage_DeletedSuccessfully_Message);
                }
            }

            return WireJson(success);
        }

        /// <summary>
        /// Suggests the languages.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Suggested languages list</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SuggestLanguages(SuggestionViewModel model)
        {
            var suggestedLanguages = GetCommand<SuggestLanguagesCommand>().ExecuteCommand(model);

            return Json(new { suggestions = suggestedLanguages });
        }
    }
}