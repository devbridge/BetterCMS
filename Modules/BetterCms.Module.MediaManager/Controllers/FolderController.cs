using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Command.Folder;
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
    /// Folder manager.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class FolderController : CmsControllerBase
    {
        /// <summary>
        /// An action to save the folder.
        /// </summary>
        /// <param name="folder">The folder data.</param>
        /// <returns>Json with status.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        [HttpPost]
        public ActionResult SaveFolder(MediaFolderViewModel folder)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveFolderCommand>().ExecuteCommand(folder);
                if (response != null)
                {
                    if (folder.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(MediaGlobalization.CreateFolder_CreatedSuccessfully_Message);
                    }
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// An action to delete a given folder.
        /// </summary>
        /// <param name="id">The folder id.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Json with status.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.DeleteContent)]
        [HttpPost]
        public ActionResult DeleteFolder(string id, string version)
        {
            bool success = GetCommand<DeleteFolderCommand>().ExecuteCommand(
                new DeleteFolderCommandRequest
                {
                    FolderId = id.ToGuidOrDefault(),
                    Version = version.ToIntOrDefault()
                });

            if (success)
            {
                Messages.AddSuccess(MediaGlobalization.DeleteFolder_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }
    }
}