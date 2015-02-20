using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Registration;
using BetterCms.Core.Security;
using BetterCms.Module.Root.Models.Sidebar;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels;
using BetterCms.Module.Root.ViewModels.Cms;

using Common.Logging;

using BetterModules.Core.Exceptions;

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
        private readonly ICmsModulesRegistration modulesRegistration;

        /// <summary>
        /// The contract for BetterCMS application host.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SidebarController" /> class.
        /// </summary>
        /// <param name="modulesRegistration">The modules.</param>
        /// <param name="configuration">The CMS configuration.</param>
        public SidebarController(ICmsModulesRegistration modulesRegistration, ICmsConfiguration configuration)
        {
            this.configuration = configuration;
            this.modulesRegistration = modulesRegistration;
        }

        /// <summary>
        /// Renders side menu container partial view.
        /// </summary>
        /// <returns>Partial view of side menu container.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Container(RenderPageViewModel renderPageViewModel)
        {
            var modelToRender = renderPageViewModel.RenderingPage ?? renderPageViewModel;
            SidebarContainerViewModel model = new SidebarContainerViewModel();

            try
            {
                model.HeaderProjections = new PageProjectionsViewModel
                    {
                        Page = modelToRender,
                        Projections = modulesRegistration.GetSidebarHeaderProjections().OrderBy(f => f.Order)
                    };

                model.SideProjections = new PageProjectionsViewModel
                    {
                        Page = modelToRender,
                        Projections = modulesRegistration.GetSidebarSideProjections().OrderBy(f => f.Order)
                    };

                model.BodyProjections = new PageProjectionsViewModel
                    {
                        Page = modelToRender,
                        Projections = modulesRegistration.GetSidebarBodyProjections().OrderBy(f => f.Order)
                    };

                model.Version = configuration.Version;
            }
            catch (CoreException ex)
            {
                Log.Error(ex);
            }                       

            return PartialView(model);
        }
    }
}
