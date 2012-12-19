using System.Web.Mvc;

using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

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
        public ActionResult Index(MediaManagerViewModel model)
        {
//            if (model.Options == null)
//            {
//                model.Options = new SearchableGridOptions { Column = "Title" };
//            }
            return View(model);
        }
    }
}