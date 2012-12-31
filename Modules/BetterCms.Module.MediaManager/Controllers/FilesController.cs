using System.Web.Mvc;

using BetterCms.Module.MediaManager.Command.Files;
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
