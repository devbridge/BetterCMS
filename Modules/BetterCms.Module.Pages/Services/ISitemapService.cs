// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISitemapService.cs" company="Devbridge Group LLC">
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
using System.Security.Principal;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Services
{
    /// <summary>
    /// Sitemap service.
    /// </summary>
    public interface ISitemapService
    {
        /// <summary>
        /// Gets specific sitemap.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier. Gets the first one if this parameter has empty GUID value.</param>
        /// <returns>The sitemap.</returns>
        Sitemap Get(Guid sitemapId);

        /// <summary>
        /// Gets specific sitemap.
        /// </summary>
        /// <param name="sitemapTitle">The sitemap title.</param>
        /// <returns>The sitemap.</returns>
        Sitemap GetByTitle(string sitemapTitle);

        /// <summary>
        /// Gets first sitemap.
        /// </summary>
        /// <returns>The sitemap.</returns>
        Sitemap GetFirst();

        /// <summary>
        /// Gets the nodes by URL.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        /// Node list.
        /// </returns>
        IList<SitemapNode> GetNodesByPage(Page page);

        /// <summary>
        /// Changes the URL.
        /// </summary>
        /// <param name="oldUrl">The old URL.</param>
        /// <param name="newUrl">The new URL.</param>
        /// <returns>Updated nodes.</returns>
        IList<SitemapNode> ChangeUrlsInAllSitemapsNodes(string oldUrl, string newUrl);

        /// <summary>
        /// Deletes the node and child nodes.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="sitemapId">The sitemap identifier.</param>
        void DeleteNode(Guid id, int version, Guid? sitemapId = null);

        /// <summary>
        /// Deletes the node and child nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        void DeleteNode(SitemapNode node, ref IList<SitemapNode> deletedNodes);

        /// <summary>
        /// Saves the node.
        /// </summary>
        /// <param name="nodeUpdated"></param>
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="macro">The macro.</param>
        /// <param name="pageId">The page identifier.</param>
        /// <param name="usePageTitleAsNodeTitle">if set to <c>true</c> [use page title as node title].</param>
        /// <param name="displayOrder">The display order.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeList"></param>
        /// <returns>
        /// Updated or newly created sitemap node.
        /// </returns>
        SitemapNode SaveNode(out bool nodeUpdated, Sitemap sitemap, Guid nodeId, int version, string url, string title, string macro, Guid pageId, bool usePageTitleAsNodeTitle, int displayOrder, Guid parentId, bool isDeleted = false, SitemapNode parentNode = null, List<SitemapNode> nodeList = null);

        /// <summary>
        /// Gets the sitemap history.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns>
        /// Sitemap previous archived versions.
        /// </returns>
        IList<SitemapArchive> GetSitemapHistory(Guid sitemapId);

        /// <summary>
        /// Archives the sitemap.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        void ArchiveSitemap(Guid sitemapId);

        /// <summary>
        /// Archives the sitemap.
        /// </summary>
        /// <param name="sitemap">The sitemap.</param>
        void ArchiveSitemap(Sitemap sitemap);

        /// <summary>
        /// Gets the archived sitemap version for preview.
        /// </summary>
        /// <param name="archiveId">The archive identifier.</param>
        /// <returns>Sitemap entity.</returns>
        Sitemap GetArchivedSitemapVersionForPreview(Guid archiveId);

        /// <summary>
        /// Restores the sitemap from archive.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <returns>Restored sitemap.</returns>
        Sitemap RestoreSitemapFromArchive(SitemapArchive archive);

        /// <summary>
        /// Deletes the sitemap.
        /// </summary>
        /// <param name="sitemapId">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="currentUser">The current user.</param>
        void DeleteSitemap(Guid sitemapId, int version, IPrincipal currentUser);
    }
}