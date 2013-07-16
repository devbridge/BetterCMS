using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    public class NodesService : Service, INodesService
    {
        private readonly IRepository repository;

        public NodesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetSitemapNodesResponse Get(GetSitemapNodesRequest request)
        {
            request.SetDefaultOrder("Title");

            var listResponse = repository
                .AsQueryable<Module.Pages.Models.SitemapNode>()
                .Select(node => new SitemapNodeModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        CreatedBy = node.CreatedByUser,
                        CreatedOn = node.CreatedOn,
                        LastModifiedBy = node.ModifiedByUser,
                        LastModifiedOn = node.ModifiedOn,

                        ParentId = node.ParentNode != null ? node.ParentNode.Id : (System.Guid?)null,
                        Title = node.Title,
                        Url = node.Url,
                        DisplayOrder = node.DisplayOrder
                    })
                .ToDataListResponse(request);

            return new GetSitemapNodesResponse { Data = listResponse };
        }
    }
}