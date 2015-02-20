using System;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    public class NodesService : Service, INodesService
    {
        private readonly IRepository repository;

        public NodesService(IRepository repository)
        {
            this.repository = repository;
        }

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
    }
}