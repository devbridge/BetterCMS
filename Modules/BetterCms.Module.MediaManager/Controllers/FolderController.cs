using System.Web.Mvc;

using BetterCms.Module.MediaManager.Command.Folder;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    public class FolderController : CmsControllerBase
    {
        /// <summary>
        /// An action to save the folder.
        /// </summary>
        /// <param name="folder">The folder data.</param>
        /// <returns>Json with status.</returns>
        [HttpPost]
        public ActionResult SaveFolder(MediaFolderViewModel folder)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveFolderCommand>().ExecuteCommand(folder);
                if (folder.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(MediaGlobalization.CreateFolder_CreatedSuccessfully_Message);
                }
                return Json(new WireJson { Success = true, Data = response });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// An action to delete a given folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>
        /// Json with status.
        /// </returns>
        [HttpPost]
        public ActionResult DeleteFolder(MediaFolderViewModel folder)
        {
            bool success = GetCommand<DeleteFolderCommand>().ExecuteCommand(
                new DeleteFolderCommandRequest
                {
                    FolderId = folder.Id,
                    Version = folder.Version
                });

            if (success)
            {
                Messages.AddSuccess(MediaGlobalization.DeleteFolder_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }
    }
}