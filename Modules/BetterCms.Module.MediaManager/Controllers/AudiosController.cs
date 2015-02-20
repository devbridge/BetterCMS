using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Command.Audios.GetAudios;
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Audio manager.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class AudiosController : CmsControllerBase
    {
        /// <summary>
        /// Gets the audios list.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        /// List of audios.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult GetAudiosList(MediaManagerViewModel options)
        {
            var success = true;
            if (options == null)
            {
                options = new MediaManagerViewModel();
            }
            options.SetDefaultPaging();

            var model = GetCommand<GetAudiosCommand>().ExecuteCommand(options);
            if (model == null)
            {
                success = false;
            }

            return Json(new WireJson { Success = success, Data = model });
        }

        /// <summary>
        /// Deletes audio.
        /// </summary>
        /// <param name="id">The audio id.</param>
        /// <param name="version">The audio entity version.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.DeleteContent)]
        [HttpPost]
        public ActionResult AudioDelete(string id, string version)
        {
            var request = new DeleteMediaCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };

            if (GetCommand<DeleteMediaCommand>().ExecuteCommand(request))
            {
                Messages.AddSuccess(MediaGlobalization.DeleteAudio_DeletedSuccessfully_Message);
                return Json(new WireJson
                {
                    Success = true
                });
            }

            return Json(new WireJson { Success = false });
        }
    }
}
