using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Registration;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Models.Sidebar;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels;

using Common.Logging;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Side menu controller.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class SidebarController : CmsControllerBase
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// A contract to manage modules registry.
        /// </summary>
        private readonly IModulesRegistration modulesRegistration;

        /// <summary>
        /// The page extensions.
        /// </summary>
        private readonly IPageAccessor pageAccessor;

        /// <summary>
        /// The contract for BetterCMS application host.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SidebarController" /> class.
        /// </summary>
        /// <param name="modulesRegistration">The modules.</param>
        /// <param name="pageAccessor">The page extensions.</param>
        /// <param name="configuration">The CMS configuration.</param>
        public SidebarController(IModulesRegistration modulesRegistration, IPageAccessor pageAccessor, ICmsConfiguration configuration)
        {
            this.configuration = configuration;
            this.modulesRegistration = modulesRegistration;
            this.pageAccessor = pageAccessor;
        }

        /// <summary>
        /// Renders side menu container partial view.
        /// </summary>
        /// <returns>Partial view of side menu container.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Container()
        {
            SidebarContainerViewModel model = new SidebarContainerViewModel();

            try
            {
                IPage page = pageAccessor.GetCurrentPage(HttpContext);
               
                model.HeaderProjections = new PageProjectionsViewModel
                    {
                        Page = page,
                        Projections = modulesRegistration.GetSidebarHeaderProjections().OrderBy(f => f.Order)
                    };

                model.SideProjections = new PageProjectionsViewModel
                    {
                        Page = page,
                        Projections = modulesRegistration.GetSidebarSideProjections().OrderBy(f => f.Order)
                    };

                model.BodyProjections = new PageProjectionsViewModel
                    {
                        Page = page,
                        Projections = modulesRegistration.GetSidebarBodyProjections().OrderBy(f => f.Order)
                    };

                model.Version = configuration.Version;
            }
            catch (CmsException ex)
            {
                Log.Error(ex);
            }                       

            return PartialView(model);
        }
    }
}
