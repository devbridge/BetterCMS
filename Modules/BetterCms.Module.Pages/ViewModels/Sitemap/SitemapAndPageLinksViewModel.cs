// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapAndPageLinksViewModel.cs" company="Devbridge Group LLC">
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
using System.Globalization;

namespace BetterCms.Module.Pages.ViewModels.Sitemap
{
    /// <summary>
    /// View model for sitemap and page links data.
    /// </summary>
    public class SitemapAndPageLinksViewModel
    {
        /// <summary>
        /// Gets or sets the page links.
        /// </summary>
        /// <value>
        /// The page links.
        /// </value>
        public IList<PageLinkViewModel> PageLinks { get; set; }

        /// <summary>
        /// Gets or sets the sitemap.
        /// </summary>
        /// <value>
        /// The sitemap.
        /// </value>
        public SitemapViewModel Sitemap { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "PageLinks count: {0}, RootNodes count: {1}",
                PageLinks != null ? PageLinks.Count.ToString(CultureInfo.InvariantCulture) : string.Empty,
                Sitemap != null && Sitemap.RootNodes != null ? Sitemap.RootNodes.Count.ToString(CultureInfo.InvariantCulture) : string.Empty);
        }
    }
}