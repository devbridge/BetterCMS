using System.Web.Mvc;

using BetterCms.Module.MediaManager.Command.Audios;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    public class AudiosController : CmsControllerBase
    {
        /// <summary>
        /// Gets the audios list.
        /// </summary>
        /// <returns>List of audios</returns>
        public ActionResult GetAudiosList(MediaManagerViewModel options)
        {
            var success = true;
            if (options == null)
            {
                options = new MediaManagerViewModel();
            }

            var model = GetCommand<GetAudiosCommand>().ExecuteCommand(options);
            if (model == null)
            {
                success = false;
            }
            return Json(new WireJson { Success = success, Data = model });
        }
    }
}
