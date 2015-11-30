// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodesService.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    /// <summary>
    /// Service contract for sitemap nodes.
    /// </summary>
    public interface INodesService
    {
        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetSitemapNodesResponse</c> with nodes list.</returns>
        GetSitemapNodesResponse Get(GetSitemapNodesRequest request);


        // NOTE: do not implement: replaces all the sitemap nodes.
        // PutSitemapNodesResponse Put(PutSitemapNodesRequest request);

        /// <summary>
        /// Creates a new sitemap node.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostSitemapNodeResponse</c> with a new sitemap node id.</returns>
        PostSitemapNodeResponse Post(PostSitemapNodeRequest request);

        // NOTE: do not implement: drops all the sitemap nodes.
        // DeleteSitemapNodesResponse Delete(DeleteSitemapNodesRequest request);
    }
}