// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapHistoryItem.cs" company="Devbridge Group LLC">
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
using System;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.History
{
    /// <summary>
    /// Sitemap history item view model.
    /// </summary>
    public class SitemapHistoryItem : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the archived on.
        /// </summary>
        /// <value>
        /// The archived on.
        /// </value>
        public DateTime? ArchivedOn { get; set; }

        /// <summary>
        /// Gets or sets the archived by user.
        /// </summary>
        /// <value>
        /// The archived by user.
        /// </value>
        public string ArchivedByUser { get; set; }

        /// <summary>
        /// Gets or sets the name of the status.
        /// </summary>
        /// <value>
        /// The name of the status.
        /// </value>
        public string StatusName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string SitemapTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current user can restore it.
        /// </summary>
        /// <value>
        /// <c>true</c> if current user can restore it; otherwise, <c>false</c>.
        /// </value>
        public bool CanCurrentUserRestoreIt { get; set; }
    }
}