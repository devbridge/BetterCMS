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
using System;
using System.Linq;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;

using BetterModules.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Category nodes service.
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
        ///   <c>GetCategoryNodesResponse</c> with nodes list.
        /// </returns>
        public GetCategoryNodesResponse Get(GetCategoryNodesRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var query = repository.AsQueryable<Module.Root.Models.Category>();
            if (request.CategoryTreeId.HasValue)
            {
                query = query.Where(node => node.CategoryTree.Id == request.CategoryTreeId);
            }
            query = query.Where(node => !node.CategoryTree.IsDeleted && !node.IsDeleted);

            var listResponse = query.Select(node => new CategoryNodeModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        CreatedBy = node.CreatedByUser,
                        CreatedOn = node.CreatedOn,
                        LastModifiedBy = node.ModifiedByUser,
                        LastModifiedOn = node.ModifiedOn,

                        CategoryTreeId = node.CategoryTree.Id,
                        ParentId = node.ParentCategory != null && !node.ParentCategory.IsDeleted ? node.ParentCategory.Id : (Guid?)null,
                        Name = node.Name,
                        DisplayOrder = node.DisplayOrder,
                        Macro = node.Macro
                    })
                .ToDataListResponse(request);

            return new GetCategoryNodesResponse { Data = listResponse };
        }

        /// <summary>
        /// Creates a new category node.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostCategoryNodeResponse</c> with a new category node id.
        /// </returns>
        public PostCategoryNodeResponse Post(PostCategoryNodeRequest request)
        {
            var result = nodeService.Put(
                    new PutNodeRequest
                    {
                        Data = request.Data,
                        User = request.User,
                        CategoryTreeId = request.CategoryTreeId
                    });

            return new PostCategoryNodeResponse { Data = result.Data };
        }
    }
}