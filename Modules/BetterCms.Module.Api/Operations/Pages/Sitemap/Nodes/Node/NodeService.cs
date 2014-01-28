using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    public class NodeService : Service, INodeService
    {
        private readonly IRepository repository;

        public NodeService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetSitemapNodeResponse Get(GetSitemapNodeRequest request)
        {
            var model = repository
                .AsQueryable<Module.Pages.Models.SitemapNode>()
                .Where(node => node.Id == request.NodeId && !node.IsDeleted)
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
                        PageLanguageId = node.Page != null && !node.Page.IsDeleted && node.Page.Language != null ? node.Page.Language.Id : (System.Guid?)null,
                        Title = node.Title,
                        Url = node.Url,
                        DisplayOrder = node.DisplayOrder,
                        Macro = node.Macro
                    })
                .FirstOne();

            return new GetSitemapNodeResponse { Data = model };
        }
    }
}