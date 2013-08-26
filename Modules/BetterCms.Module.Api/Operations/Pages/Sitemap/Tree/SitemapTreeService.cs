using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    public class SitemapTreeService : Service, ISitemapTreeService
    {
        private readonly IRepository repository;

        public SitemapTreeService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetSitemapTreeResponse Get(GetSitemapTreeRequest request)
        {
            var allNodes = repository
                .AsQueryable<Module.Pages.Models.SitemapNode>()
                .OrderBy(node => node.Title)
                .Select(node => new SitemapTreeNodeModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        CreatedBy = node.CreatedByUser,
                        CreatedOn = node.CreatedOn,
                        LastModifiedBy = node.ModifiedByUser,
                        LastModifiedOn = node.ModifiedOn,

                        ParentId = node.ParentNode != null && !node.ParentNode.IsDeleted ? node.ParentNode.Id : (System.Guid?)null,
                        Title = node.Title,
                        Url = node.Url,
                        DisplayOrder = node.DisplayOrder
                    })
                .ToList();

            return new GetSitemapTreeResponse { Data = GetChildren(allNodes, request.Data.NodeId) };
        }

        private List<SitemapTreeNodeModel> GetChildren(List<SitemapTreeNodeModel> allItems, System.Guid? parentId)
        {
            var childItems = allItems.Where(item => item.ParentId == parentId && item.Id != parentId).ToList();

            foreach (var item in childItems)
            {
                item.ChildrenNodes = GetChildren(allItems, item.Id);
            }

            return childItems;
        }
    }
}