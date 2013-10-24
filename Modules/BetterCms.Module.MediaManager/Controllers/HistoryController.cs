using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Command.History.GetMediaHistory;
using BetterCms.Module.MediaManager.Command.History.GetMediaVersion;
using BetterCms.Module.MediaManager.Command.History.RestoreMediaVersion;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Media history controller.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class HistoryController : CmsControllerBase
    {
        /// <summary>
        /// Medias the history.
        /// </summary>
        /// <param name="mediaId">The content id.</param>
        /// <returns>Media history view.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.EditContent)]
        public ActionResult MediaHistory(string mediaId)
        {
            var model = GetCommand<GetMediaHistoryCommand>().ExecuteCommand(new GetMediaHistoryRequest
                {
                    MediaId = mediaId.ToGuidOrDefault(),
                });

            return View(model);
        }

        /// <summary>
        /// Medias the history.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Media history view.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult MediaHistory(GetMediaHistoryRequest request)
        {
            var model = GetCommand<GetMediaHistoryCommand>().ExecuteCommand(request);

            return View(model);
        }

        /// <summary>
        /// Medias the version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Media preview.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.EditContent)]
        public ActionResult MediaVersion(string id)
        {
            var model = GetCommand<GetMediaVersionCommand>().ExecuteCommand(id.ToGuidOrDefault());
           
            return View(model);
        }

        /// <summary>
        /// Restores the media version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>WireJson result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult RestoreMediaVersion(string id)
        {
            var result = GetCommand<RestoreMediaVersionCommand>().ExecuteCommand(id.ToGuidOrDefault());
            
            return WireJson(result);
        }
    }
}
