using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Vimeo.Command.Videos.GetVideosToSelect;
using BetterCms.Module.Vimeo.Command.Videos.SaveVideos;
using BetterCms.Module.Vimeo.Content.Resources;
using BetterCms.Module.Vimeo.ViewModels.Video;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Vimeo.Controllers
{
    [BcmsAuthorize]
    [ActionLinkArea(VimeoModuleDescriptor.VimeoAreaName)]
    public class VideosController : CmsControllerBase
    {
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult Search(VideoListSearchViewModel request)
        {
            if (request == null)
            {
                request = new VideoListSearchViewModel();
            }

            var model = GetCommand<GetVideosToSelectCommand>().ExecuteCommand(request);
            var success = model != null;
            var view = RenderView("Search", new VideoViewModel());

            return ComboWireJson(success, view, model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveVideos(SaveVideosRequest model)
        {
            var result = GetCommand<SaveVideosCommand>().ExecuteCommand(model);

            if (result == null)
            {
                Messages.AddError(VimeoGlobalization.SaveVideos_SaveFailed);
            }
            else if (result.FolderIsDeleted)
            {
                Messages.AddError(VimeoGlobalization.SaveVideos_SaveFailed_FolderDeleted);
            }

            return Json(new WireJson(result != null, result));
        }
    }
}