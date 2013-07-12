using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Viddler.Command.Videos.SaveVideos;
using BetterCms.Module.Viddler.Content.Resources;
using BetterCms.Module.Viddler.ViewModels.Video;

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
    }
}