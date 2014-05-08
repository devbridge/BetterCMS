using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    public class SitemapService : Service, ISitemapService
    {
        private readonly IRepository repository;

        private readonly ISitemapTreeService treeService;

        private readonly INodesService nodesService;

        private readonly INodeService nodeService;

        public SitemapService(IRepository repository, ISitemapTreeService treeService, INodeService nodeService, INodesService nodesService)
        {
            this.repository = repository;
            this.treeService = treeService;
            this.nodeService = nodeService;
            this.nodesService = nodesService;
        }

        ISitemapTreeService ISitemapService.Tree
        {
            get
            {
                return treeService;
            }
        }

        INodesService ISitemapService.Nodes
        {
            get
            {
                return nodesService;
            }
        }

        INodeService ISitemapService.Node
        {
            get
            {
                return nodeService;
            }
        }

        public GetSitemapResponse Get(GetSitemapRequest request)
        {
            var sitemap =
                repository.AsQueryable<Module.Pages.Models.Sitemap>()
                    .Where(s => s.Id == request.SitemapId)
                    .Select(
                        s =>
                        new SitemapModel
                            {
                                Id = s.Id,
                                CreatedBy = s.CreatedByUser,
                                CreatedOn = s.CreatedOn,
                                LastModifiedBy = s.ModifiedByUser,
                                LastModifiedOn = s.ModifiedOn,
                                Version = s.Version,
                                Title = s.Title,
                                Tags = s.SitemapTags.Select(t => t.Tag.Name).ToList()
                            })
                    .FirstOne();

            return new GetSitemapResponse { Data = sitemap };
        }


        public PutSitemapResponse Put(PutSitemapRequest request)
        {
            throw new System.NotImplementedException();
        }

        public DeleteSitemapResponse Delete(DeleteSitemapRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}