using System.Linq;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Registration;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Models.SiteSettingsMenu;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels;
using BetterModules.Core.Exceptions;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Site settings menu controller.
    /// </summary>
    [BcmsAuthorize]
    [Area(RootModuleDescriptor.RootAreaName)]
    public class SiteSettingsController : CmsControllerBase
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// A contract to manage modules registry.
        /// </summary>
        private readonly ICmsModulesRegistration modulesRegistration;

        /// <summary>
        /// The page extensions.
        /// </summary>
        private readonly IPageAccessor pageAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteSettingsController" /> class.
        /// </summary>
        /// <param name="modulesRegistration">The modules registration.</param>
        /// <param name="pageAccessor">The page extensions.</param>
        /// <param name="loggerFactory">The logger factory</param>
        public SiteSettingsController(ICmsModulesRegistration modulesRegistration, IPageAccessor pageAccessor, 
            ILoggerFactory loggerFactory, ISecurityService securityService) : base(securityService)
        {
            this.modulesRegistration = modulesRegistration;
            this.pageAccessor = pageAccessor;
            logger = loggerFactory.CreateLogger<SiteSettingsController>();
        }

        /// <summary>
        /// Renders site setting menu container partial view.
        /// </summary>
        /// <returns>Partial view of site settings menu container.</returns>
        public IActionResult Container()
        {
            SiteSettingsContainerViewModel model = new SiteSettingsContainerViewModel();

            try
            {
                var siteSettingsProjections = modulesRegistration.GetSiteSettingsProjections();
                if (siteSettingsProjections != null)
                {
                    model.MenuItems = new PageProjectionsViewModel();
                    model.MenuItems.Page = pageAccessor.GetCurrentPage(HttpContext);
                    model.MenuItems.Projections = siteSettingsProjections.OrderBy(f => f.Order);                   
                }
            }
            catch (CoreException ex)
            {
                logger.LogError("Failed to load site settings container data.", ex);
            }

            return View(model);
        }
    }
}
