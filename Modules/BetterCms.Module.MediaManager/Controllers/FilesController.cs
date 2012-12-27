using System.Web.Mvc;

using BetterCms.Module.MediaManager.Command.Folder;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Handles site settings logic for Media module Files tab.
    /// </summary>
    public class FilesController : CmsControllerBase
    {
        /// <summary>
        /// Files tab.
        /// </summary>
        /// <returns>
        /// The view.
        /// </returns>
        public ActionResult FilesTab()
        {
            return PartialView();
        }

        /// <summary>
        /// Gets the files list.
        /// </summary>
        /// <returns>List of files</returns>
        public ActionResult GetFilesList(MediaManagerViewModel options)
        {
            var success = true;
            if (options == null)
            {
                options = new MediaManagerViewModel();
            }

            var model = GetCommand<GetFilesCommand>().ExecuteCommand(options);
            if (model == null)
            {
                success = false;
            }
            return Json(new WireJson { Success = success, Data = model });
        }
    }
}
