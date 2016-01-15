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
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Service contract for sitemap operations.
    /// </summary>
    public interface ISitemapService
    {
        /// <summary>
        /// Gets the tree.
        /// </summary>
        /// <value>
        /// The tree.
        /// </value>
        ISitemapTreeService Tree { get; }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        INodesService Nodes { get; }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        INodeService Node { get; }

        /// <summary>
        /// Gets the sitemap specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetSitemapsResponse</c> with sitemap data.</returns>
        GetSitemapResponse Get(GetSitemapRequest request);

        /// <summary>
        /// Puts the sitemap specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutSitemapsResponse</c> with updated sitemap data.</returns>
        PutSitemapResponse Put(PutSitemapRequest request);

        /// <summary>
        /// Deletes the sitemap specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteSitemapsResponse</c> with success status.</returns>
        DeleteSitemapResponse Delete(DeleteSitemapRequest request);
    }
}