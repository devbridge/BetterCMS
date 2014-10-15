using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.Setting;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Handles configuration settings logic.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class SettingController : CmsControllerBase
    {
        /// <summary>
        /// Gets settings list for Site Settings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Settings list.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult Settings(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetSettingsListCommand>().ExecuteCommand(request);
            var success = model != null;

            var view = RenderView("Settings", model);
            
            return ComboWireJson(success, view, null, JsonRequestBehavior.AllowGet);
        }
    }
}