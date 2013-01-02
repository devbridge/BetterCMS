using System.Web.Mvc;

using BetterCms.Module.MediaManager.Command.Videos.GetVideos;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    public class VideosController : CmsControllerBase
    {
        /// <summary>
        /// Gets the videos list.
        /// </summary>
        /// <returns>List of videos</returns>
        public ActionResult GetVideosList(MediaManagerViewModel options)
        {
            var success = true;
            if (options == null)
            {
                options = new MediaManagerViewModel();
            }

            var model = GetCommand<GetVideosCommand>().ExecuteCommand(options);
            if (model == null)
            {
                success = false;
            }
            return Json(new WireJson { Success = success, Data = model });
        }
    }
}
