using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.Tag.GetTagList;
using BetterCms.Module.Pages.Commands.DeleteTag;
using BetterCms.Module.Pages.Commands.SaveTag;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Tags;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Handles site settings logic for Pages module.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class TagsController : CmsControllerBase
    {
        /// <summary>
        /// Renders a tag list for the site settings dialog.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered tag list.
        /// </returns>
        public ActionResult ListTags(SearchableGridOptions request)
        {
            var model = GetCommand<GetTagListCommand>().ExecuteCommand(request);
            return View(model);
        }

        /// <summary>
        /// An action to save the tag.
        /// </summary>
        /// <param name="tag">The tag data.</param>
        /// <returns>Json with status.</returns>
        [HttpPost]
        public ActionResult SaveTag(TagItemViewModel tag)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveTagCommand>().ExecuteCommand(tag);
                if (response != null)
                {
                    if (tag.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(PagesGlobalization.CreateTag_CreatedSuccessfully_Message);
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
                Messages.AddSuccess(PagesGlobalization.DeleteTag_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }
    }
}
