using System;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Viddler.Command.UplaodVideos;
using BetterCms.Module.Viddler.Command.Videos.SaveVideos;
using BetterCms.Module.Viddler.Content.Resources;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Viddler.Controllers
{
    [BcmsAuthorize]
    [ActionLinkArea(ViddlerModuleDescriptor.ViddlerAreaName)]
    public class VideosController : CmsControllerBase
    {
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

        public ActionResult UplaodVideos(string folderId, string reuploadMediaId)
        {
            var model = GetCommand<GetVideoUploadCommand>().ExecuteCommand(
                new GetVideoUploadRequest
                {
                    FolderId = folderId.ToGuidOrDefault(),
                    ReuploadMediaId = reuploadMediaId.ToGuidOrDefault()
                });
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

            var view = RenderView("UplaodVideos", model);
            return ComboWireJson(success, view, model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VideoUploaded()
        {
            return null;
        }
    }
}