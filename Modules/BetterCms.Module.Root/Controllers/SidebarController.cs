// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SidebarController.cs" company="Devbridge Group LLC">
//
// Copyright (C) 2015,2016 Devbridge Group LLC
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
//
// Website: https://www.bettercms.com
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            var model = new SidebarContainerViewModel();

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
