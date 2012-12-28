using System.Web.Mvc;

using BetterCms.Module.MediaManager.Command.Images;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Handles site settings logic to operate with a media manager.
    /// </summary>
    public class MediaManagerController : CmsControllerBase
    {
        /// <summary>
        /// Renders a media manager tabs container.
        /// </summary>
        /// <returns>
        /// Rendered media manager tabs container.
        /// </returns>
        public ActionResult Index()
        {
            var images = GetCommand<GetImagesCommand>().ExecuteCommand(new MediaManagerViewModel());
            var json = new
                           {
                               Data = new WireJson
                                          {
                                              Success = true, 
                                              Data = images
                                          }, 
                               Html = RenderView("Index", new MediaImageViewModel())
                           };
            return WireJson(true, json, JsonRequestBehavior.AllowGet);
        }
    }
}