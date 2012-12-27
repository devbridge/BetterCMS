using System.Web.Mvc;
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
            return View();
        }
    }
}