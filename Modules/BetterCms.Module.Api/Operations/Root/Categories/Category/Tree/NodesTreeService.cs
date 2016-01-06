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