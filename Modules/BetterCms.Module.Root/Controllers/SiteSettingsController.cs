// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteSettingsController.cs" company="Devbridge Group LLC">
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
using System.Linq;
using System.Web.Mvc;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Registration;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Models.SiteSettingsMenu;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels;

using Common.Logging;

using BetterModules.Core.Exceptions;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Site settings menu controller.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class SiteSettingsController : CmsControllerBase
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
        /// The page extensions.
        /// </summary>
        private readonly IPageAccessor pageAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteSettingsController" /> class.
        /// </summary>
        /// <param name="modulesRegistration">The modules registration.</param>
        /// <param name="pageAccessor">The page extensions.</param>
        public SiteSettingsController(ICmsModulesRegistration modulesRegistration, IPageAccessor pageAccessor)
        {
            this.modulesRegistration = modulesRegistration;
            this.pageAccessor = pageAccessor;
        }

        /// <summary>
        /// Renders site setting menu container partial view.
        /// </summary>
        /// <returns>Partial view of site settings menu container.</returns>
        public ActionResult Container()
        {
            var model = new SiteSettingsContainerViewModel();

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
                Log.Error("Failed to load site settings container data.", ex);
            }

            return View(model);
        }
    }
}
