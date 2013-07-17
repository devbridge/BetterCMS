using System;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Viddler.Command.UplaodVideos;
using BetterCms.Module.Viddler.Command.ViddlerData;
using BetterCms.Module.Viddler.Command.Videos.SaveVideos;
using BetterCms.Module.Viddler.Content.Resources;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Viddler.Controllers
{
    /// <summary>
    /// Video provider main controller.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(ViddlerModuleDescriptor.ViddlerAreaName)]
    public class VideosController : CmsControllerBase
    {
        /// <summary>
        /// Upload the videos.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <param name="reuploadMediaId">The re-upload media id.</param>
        /// <returns>Combo Json data.</returns>
        public ActionResult UploadVideos(string folderId, string reuploadMediaId)
        {
            var model = GetCommand<GetVideoUploadCommand>().ExecuteCommand(
                new GetVideoUploadRequest
                {
                    FolderId = folderId.ToGuidOrDefault(),
                    ReuploadMediaId = reuploadMediaId.ToGuidOrDefault()
                });
            var success = model != null;

            var view = RenderView("UploadVideos", model);
            return ComboWireJson(success, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the data for upload.
        /// </summary>
        /// <returns>Json result.</returns>
        public ActionResult GetViddlerDataForUpload()
        {
            var model = GetCommand<GetViddlerDataForUploadCommand>().ExecuteCommand();
            var success = model != null;
            if (success)
            {
                UrlHelper u = new UrlHelper(ControllerContext.RequestContext);
                var url = HttpUtility.UrlDecode(u.Action("VideoUploaded", "Videos", null));
                model.CallbackUrl = url;
// TODO: remove if above works correctly.
//            var requestUrl = HttpContext.Request.Url;
//            if (requestUrl != null)
//            {
//                var isCustomPort = requestUrl.Scheme == Uri.UriSchemeHttp && requestUrl.Port != 80 || requestUrl.Scheme == Uri.UriSchemeHttps && requestUrl.Port != 433;
//                url = string.Concat(requestUrl.Scheme, "://", requestUrl.Host, isCustomPort ? ":" + requestUrl.Port : string.Empty, url);
//                model.CallbackUrl = url;
//            }
            }

            return ComboWireJson(success, null, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Callback from video service provides after successful video upload.
        /// </summary>
        /// <returns>The response.</returns>
        public WrappedJsonResult VideoUploaded(string video_id)
        {
            // TODO: implement callback from Viddler.
            var media = new MediaFile();
            System.Threading.Thread.Sleep(5000);
            return new WrappedJsonResult
            {
                Data = new
                {
                    Success = true,
                    Id = media.Id,
                    fileName = media.OriginalFileName,
                    fileSize = media.Size,
                    Version = media.Version,
                    Type = MediaType.Video,
                    IsProcessing = !media.IsUploaded.HasValue,
                    IsFailed = media.IsUploaded == false,
                }
            };
        }

        /// <summary>
        /// Saves the videos.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        public ActionResult SaveVideos(SaveVideosRequest model)
        {
            var result = GetCommand<SaveVideosCommand>().ExecuteCommand(model);

            if (result == null)
            {
                Messages.AddError(ViddlerGlobalization.SaveVideos_SaveFailed);
            }
            else if (result.FolderIsDeleted)
            {
                Messages.AddError(ViddlerGlobalization.SaveVideos_SaveFailed_FolderDeleted);
            }

            return Json(new WireJson(result != null, result));
        }
    }
}