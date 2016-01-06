using System;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Tree;
using BetterCms.Module.Pages.Models;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
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

        public GetSitemapsResponse Get(GetSitemapsRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Module.Pages.Models.Sitemap>();

            query = query.ApplySitemapTagsFilter(request.Data);

            var listResponse = query
                .Where(map => !map.IsDeleted)
                .Select(map => new SitemapModel
                {
                    Id = map.Id,
                    Version = map.Version,
                    CreatedBy = map.CreatedByUser,
                    CreatedOn = map.CreatedOn,
                    LastModifiedBy = map.ModifiedByUser,
                    LastModifiedOn = map.ModifiedOn,

                    Title = map.Title
                }).ToDataListResponse(request);

            if (listResponse.Items.Count > 0 && request.Data.IncludeTags)
            {
                LoadTags(listResponse, request.Data.IncludeTags);
            }

            return new GetSitemapsResponse
            {
                Data = listResponse
            };
        }

        private void LoadTags(DataListResponse<SitemapModel> response, bool includeTags)
        {
            var pageIds = response.Items.Select(i => i.Id).Distinct().ToArray();
            if (includeTags)
            {
                var tags = repository
                    .AsQueryable<SitemapTag>(pt => pageIds.Contains(pt.Sitemap.Id))
                    .Select(pt => new { PageId = pt.Sitemap.Id, TagName = pt.Tag.Name })
                    .OrderBy(o => o.TagName)
                    .ToFuture()
                    .ToList();

                response.Items.ToList().ForEach(page => { page.Tags = tags.Where(tag => tag.PageId == page.Id).Select(tag => tag.TagName).ToList(); });
            }
        }
    }
}