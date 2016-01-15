// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodesService.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    /// <summary>
    /// Sitemap nodes service.
    /// </summary>
    public class NodesService : Service, INodesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The node service.
        /// </summary>
        private readonly INodeService nodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodesService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="nodeService">The node service.</param>
        public NodesService(IRepository repository, INodeService nodeService)
        {
            this.repository = repository;
            this.nodeService = nodeService;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetSitemapNodesResponse</c> with nodes list.
        /// </returns>
        public GetSitemapNodesResponse Get(GetSitemapNodesRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var listResponse = repository
                .AsQueryable<Module.Pages.Models.SitemapNode>()
                .Where(node => node.Sitemap.Id == request.SitemapId && !node.IsDeleted)
                .Select(node => new SitemapNodeModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        CreatedBy = node.CreatedByUser,
                        CreatedOn = node.CreatedOn,
                        LastModifiedBy = node.ModifiedByUser,
                        LastModifiedOn = node.ModifiedOn,

                        ParentId = node.ParentNode != null && !node.ParentNode.IsDeleted ? node.ParentNode.Id : (System.Guid?)null,
                        PageId = node.Page != null && !node.Page.IsDeleted ? node.Page.Id : (System.Guid?)null,
                        PageIsPublished = node.Page != null && !node.Page.IsDeleted && node.Page.Status == PageStatus.Published,
                        PageLanguageId = node.Page != null && !node.Page.IsDeleted && node.Page.Language != null ? node.Page.Language.Id : (System.Guid?)null,
                        Title = node.Page != null && node.UsePageTitleAsNodeTitle ? node.Page.Title : node.Title,
                        Url = node.Page != null ? node.Page.PageUrl : node.Url,
                        DisplayOrder = node.DisplayOrder,
                        Macro = node.Macro
                    })
                .ToDataListResponse(request);

            return new GetSitemapNodesResponse { Data = listResponse };
        }

        /// <summary>
        /// Creates a new sitemap node.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostSitemapNodeResponse</c> with a new sitemap node id.
        /// </returns>
        public PostSitemapNodeResponse Post(PostSitemapNodeRequest request)
        {
            var result = nodeService.Put(
                    new PutNodeRequest
                    {
                        Data = request.Data,
                        User = request.User,
                        SitemapId = request.SitemapId
                    });

            return new PostSitemapNodeResponse { Data = result.Data };
        }
    }
}