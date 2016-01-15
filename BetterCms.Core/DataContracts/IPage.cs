// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPage.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.DataContracts.Enums;

using BetterModules.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic page properties.
    /// </summary>
    public interface IPage : IEntity
    {
        /// <summary>
        /// Gets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        PageStatus Status { get; }

        /// <summary>
        /// Gets a value indicating whether this page has SEO meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page has SEO; otherwise, <c>false</c>.
        /// </value>
        bool HasSEO { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }

        /// <summary>
        /// Gets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        string PageUrl { get; }

        /// <summary>
        /// Gets a value indicating whether page is master page.
        /// </summary>
        /// <value>
        /// <c>true</c> if page is master page; otherwise, <c>false</c>.
        /// </value>
        bool IsMasterPage { get; }

        /// <summary>
        /// Gets the type of the access (http vs https).
        /// </summary>
        /// <value>
        /// The type of the access (http vs https).
        /// </value>
        ForceProtocolType ForceAccessProtocol { get; }
    }
}
