using System.Web.Mvc;

using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Command.Videos.GetVideos;
using BetterCms.Module.MediaManager.Content.Resources;

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

        /// <summary>
        /// Deletes video.
        /// </summary>
        /// <param name="id">The video id.</param>
        /// <param name="version">The video entity version.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        public ActionResult VideoDelete(string id, string version)
        {
            var request = new DeleteMediaCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };
            if (GetCommand<DeleteMediaCommand>().ExecuteCommand(request))
            {
                Messages.AddSuccess(MediaGlobalization.DeleteVideo_DeletedSuccessfully_Message);
                return Json(new WireJson
                {
                    Success = true
                });
            }

            return Json(new WireJson { Success = false });
        }
    }
}
