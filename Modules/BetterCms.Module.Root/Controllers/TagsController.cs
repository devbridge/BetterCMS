using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.Root.Commands.Tag.DeleteTag;
using BetterCms.Module.Root.Commands.Tag.GetTagList;
using BetterCms.Module.Root.Commands.Tag.SaveTag;
using BetterCms.Module.Root.Commands.Tag.SearchTags;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.Autocomplete;
using BetterCms.Module.Root.ViewModels.Tags;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Handles site settings logic for Pages module.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class TagsController : CmsControllerBase
    {
        /// <summary>
        /// Renders a tag list for the site settings dialog.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered tag list.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult ListTags(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetTagListCommand>().ExecuteCommand(request);
            return View(model);
        }

        /// <summary>
        /// An action to save the tag.
        /// </summary>
        /// <param name="tag">The tag data.</param>
        /// <returns>Json with status.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult SaveTag(TagItemViewModel tag)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveTagCommand>().ExecuteCommand(tag);
                if (response != null)
                {
                    if (tag.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(RootGlobalization.CreateTag_CreatedSuccessfully_Message);
                    }
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// An action to delete a given tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>
        /// Json with status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult DeleteTag(TagItemViewModel tag)
        {
            bool success = GetCommand<DeleteTagCommand>().ExecuteCommand(                
                new DeleteTagCommandRequest
                    {
                        TagId = tag.Id,
                        Version = tag.Version
                    });

            if (success)
            {
                Messages.AddSuccess(RootGlobalization.DeleteTag_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }

        [HttpPost]
        public ActionResult SuggestTags(SuggestionViewModel model)
        {
            var suggestedTags = GetCommand<SearchTagsCommand>().ExecuteCommand(model);
            return Json(new { suggestions = suggestedTags });
        }
    }
}
