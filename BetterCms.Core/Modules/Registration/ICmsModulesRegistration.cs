// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICmsModulesRegistration.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Core.Modules.Projections;

using BetterModules.Core.Web.Modules.Registration;

namespace BetterCms.Core.Modules.Registration
{
    /// <summary>
    /// Defines the contract for modules registration logic.
    /// </summary>
    public interface ICmsModulesRegistration : IWebModulesRegistration
    {
        /// <summary>
        /// Gets the list of CMS modules.
        /// </summary>
        /// <returns>The lit of CMS modules</returns>
        List<CmsModuleDescriptor> GetCmsModules();

        /// <summary>
        /// Gets all known JS modules.
        /// </summary>
        /// <returns>Enumerator of known JS modules.</returns>
        IEnumerable<JsIncludeDescriptor> GetJavaScriptModules();

        /// <summary>
        /// Gets the style sheet files.
        /// </summary>
        /// <returns>Enumerator of known modules style sheet files.</returns>
        IEnumerable<CssIncludeDescriptor> GetStyleSheetIncludes();

        /// <summary>
        /// Gets action projections to render in the sidebar header.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the sidebar header.</returns>
        IEnumerable<IPageActionProjection> GetSidebarHeaderProjections();

        /// <summary>
        /// Gets action projections to render in the sidebar side.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the sidebar side.</returns>
        IEnumerable<IPageActionProjection> GetSidebarSideProjections();

        /// <summary>
        /// Gets action projections to render in the sidebar body container.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the sidebar body container.</returns>
        IEnumerable<IPageActionProjection> GetSidebarBodyProjections();

        /// <summary>
        /// Gets action projections to render in the site settings menu container.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the site settings menu container.</returns>
        IEnumerable<IPageActionProjection> GetSiteSettingsProjections();
    }
}
