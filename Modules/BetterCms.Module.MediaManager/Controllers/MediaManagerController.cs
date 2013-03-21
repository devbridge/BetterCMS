using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Command.Images.GetImages;
using BetterCms.Module.MediaManager.Command.MediaManager.RenameMedia;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Handles site settings logic to operate with a media manager.
    /// </summary>
    [BcmsAuthorize]
    public class MediaManagerController : CmsControllerBase
    {
        /// <summary>
        /// Renders a media manager tabs container.
        /// </summary>
        /// <returns>
        /// Rendered media manager tabs container.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult Index()
        {
            var images = GetCommand<GetImagesCommand>().ExecuteCommand(new MediaManagerViewModel());
            var success = images != null;
            var view = RenderView("Index", new MediaImageViewModel());
            
            return ComboWireJson(success, view, images, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Renames media.
        /// </summary>
        /// <param name="media">The media data.</param>
        /// <returns>Json with status.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult RenameMedia(MediaViewModel media)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<RenameMediaCommand>().ExecuteCommand(media);
                if (response != null)
                {
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }
    }
}