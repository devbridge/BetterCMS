// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITagService.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Services
{
    public interface ITagService
    {
        /// <summary>
        /// Saves the page tags.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newCreatedTags">A list of new tags.</param>
        void SavePageTags(PageProperties page, IList<string> tags, out IList<Tag> newCreatedTags);

        /// <summary>
        /// Gets the future query for the page tag names.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>The future query for the list fo tag names</returns>
        IEnumerable<string> GetPageTagNames(Guid pageId);

        /// <summary>
        /// Gets the sitemap tag names.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns></returns>
        IEnumerable<string> GetSitemapTagNames(Guid sitemapId);

        /// <summary>
        /// Saves the tags.
        /// </summary>
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newTags">The new tags.</param>
        void SaveTags(Sitemap sitemap, IList<string> tags, out IList<Tag> newTags);

        /// <summary>
        /// Saves the media tags.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newTags">The new tags.</param>
        void SaveMediaTags(Media media, IList<string> tags, out IList<Tag> newTags);
    }
}