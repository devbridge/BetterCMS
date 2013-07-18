using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Viddler.Command.DeleteVideo;
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
                var requestUrl = HttpContext.Request.Url;
                if (requestUrl != null)
                {
                    var isCustomPort = (requestUrl.Scheme == Uri.UriSchemeHttp && requestUrl.Port != 80)
                                       || (requestUrl.Scheme == Uri.UriSchemeHttps && requestUrl.Port != 433);
                    url = string.Concat(requestUrl.Scheme, "://", requestUrl.Host, isCustomPort ? ":" + requestUrl.Port : string.Empty, url);
                    model.CallbackUrl = url;
                }

                model.CallbackUrl = string.Format("{0}?token={1}", model.CallbackUrl, model.Token);

                if (HttpRuntime.Cache[model.Token] != null)
                {
                    HttpRuntime.Cache.Remove(model.Token);
                }

                HttpRuntime.Cache.Add(model.Token, true, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }

            return ComboWireJson(success, null, model, JsonRequestBehavior.AllowGet);
        }

#if DEBUG
        // TODO: REMOVE this when test account will be available.
        Random rnd = new Random();
        public ActionResult ViddlerUploadMockup(string uploadtoken, string callback, string title, HttpPostedFileWrapper file)
        {
            System.Threading.Thread.Sleep(5000);
            return rnd.Next(1, 7) < 2
                ? null // Failure imitation.
                : Redirect(string.Format("{0}?videoid=f6752122&token={1}", callback, uploadtoken));
        }
#endif

        /// <summary>
        /// Callback from video service provides after successful video upload.
        /// </summary>
        /// <returns>The response.</returns>
        public WrappedJsonResult VideoUploaded(string videoid, string token)
        {
            if (HttpRuntime.Cache[token] == null)
            {
                return null;
            }
            HttpRuntime.Cache.Remove(token);

            var video = GetCommand<SaveVideoCommand>().ExecuteCommand(
                new SaveVideoRequest
                    {
                        VideoId = videoid
                    });

            if (video == null)
            {
                return null;
            }

            return new WrappedJsonResult
                {
                    Data =
                        new
                            {
                                Success = true,
                                Id = video.Id,
                                FileName = video.Title,
                                FileSize = video.Size,
                                Version = video.Version,
                                Type = MediaType.Video,
                                IsProcessing = !video.IsUploaded.HasValue,
                                IsFailed = video.IsUploaded == false,
                            },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
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

        public ActionResult DeleteVideo(string id, string version)
        {
            var request = new DeleteMediaCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };
            if (GetCommand<DeleteVideoCommand>().ExecuteCommand(request))
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