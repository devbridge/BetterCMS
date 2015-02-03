using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    public class CategoryTreeService : Service, ICategoryTreeService
    {
        private readonly IRepository repository;

        private readonly ICmsConfiguration cmsConfiguration;

        public CategoryTreeService(IRepository repository, ICmsConfiguration cmsConfiguration)
        {
            this.repository = repository;
            this.cmsConfiguration = cmsConfiguration;
        }

        public GetCategoryTreeResponse Get(GetCategoryTreeRequest request)
        {
            var allNodes = repository
                .AsQueryable<Module.Root.Models.Category>()
                .Where(node => node.CategoryTree.Id == request.CategoryId && !node.IsDeleted)
                .OrderBy(node => node.DisplayOrder)
                .Select(node => new CategoryTreeNodeModel
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

            return new GetCategoryTreeResponse { Data = nodes };
        }

        private static List<CategoryTreeNodeModel> GetChildren(List<CategoryTreeNodeModel> allItems, Guid? parentId)
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