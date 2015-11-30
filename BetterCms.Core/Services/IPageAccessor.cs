// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPageAccessor.cs" company="Devbridge Group LLC">
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
using System.Linq;
using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Core.Services
{
    /// <summary>
    /// Defines contract to manage pages.
    /// </summary>
    public interface IPageAccessor
    {
        /// <summary>
        /// Gets current page.
        /// </summary>
        /// <returns>Current page object.</returns>
        IPage GetCurrentPage(HttpContextBase httpContext);

        /// <summary>
        /// Gets current page by given virtual path.
        /// </summary>
        /// <returns>Current page object by given virtual path.</returns>
        IPage GetPageByVirtualPath(string virtualPath);

        /// <summary>
        /// Gets the redirect.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Redirect URL</returns>
        string GetRedirect(string virtualPath);

        /// <summary>
        /// Gets the list of meta data projections.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>The list of meta data projections</returns>
        IList<IPageActionProjection> GetPageMetaData(IPage page);

        /// <summary>
        /// Gets the page query.
        /// </summary>
        /// <returns></returns>
        IQueryable<IPage> GetPageQuery();

        /// <summary>
        /// Caches the page.
        /// </summary>
        /// <param name="page">The page.</param>
        void CachePage(IPage page);
    }
}
