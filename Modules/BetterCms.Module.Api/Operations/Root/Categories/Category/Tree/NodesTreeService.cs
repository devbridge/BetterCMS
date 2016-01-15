// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodesTreeService.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    public class NodesTreeService : Service, INodesTreeService
    {
        private readonly IRepository repository;

        private readonly ICmsConfiguration cmsConfiguration;

        public NodesTreeService(IRepository repository, ICmsConfiguration cmsConfiguration)
        {
            this.repository = repository;
            this.cmsConfiguration = cmsConfiguration;
        }

        public GetNodesTreeResponse Get(GetNodesTreeRequest request)
        {
            var allNodes = repository
                .AsQueryable<Module.Root.Models.Category>()
                .Where(node => node.CategoryTree.Id == request.CategoryTreeId && !node.IsDeleted)
                .OrderBy(node => node.DisplayOrder)
                .Select(node => new NodesTreeNodeModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        CreatedBy = node.CreatedByUser,
                        CreatedOn = node.CreatedOn,
                        LastModifiedBy = node.ModifiedByUser,
                        LastModifiedOn = node.ModifiedOn,

                        ParentId = node.ParentCategory != null && !node.ParentCategory.IsDeleted ? node.ParentCategory.Id : (Guid?)null,
                        Name = node.Name,
                        DisplayOrder = node.DisplayOrder,
                        Macro = node.Macro
                    })
                .ToFuture()
                .ToList();

            var nodes = GetChildren(allNodes, request.Data.NodeId);

            return new GetNodesTreeResponse { Data = nodes };
        }

        private static List<NodesTreeNodeModel> GetChildren(List<NodesTreeNodeModel> allItems, Guid? parentId)
        {
            var childItems = allItems.Where(item => item.ParentId == parentId && item.Id != parentId).OrderBy(node => node.DisplayOrder).ToList();

            foreach (var item in childItems)
            {
                item.ChildrenNodes = GetChildren(allItems, item.Id);
            }

            return childItems;
        }
    }
}