using System;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
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
                        PageIsPublished = node.Page != null && !node.Page.IsDeleted && node.Page.Status == PageStatus.Published,
                        PageLanguageId = node.Page != null && !node.Page.IsDeleted && node.Page.Language != null ? node.Page.Language.Id : (System.Guid?)null,
                        Title = node.Page != null && node.UsePageTitleAsNodeTitle ? node.Page.Title : node.Title,
                        Url = node.Page != null ? node.Page.PageUrl : node.Url,
                        DisplayOrder = node.DisplayOrder,
                        Macro = node.Macro
                    })
                .FirstOne();

            return new GetSitemapNodeResponse { Data = model };
        }
    }
}