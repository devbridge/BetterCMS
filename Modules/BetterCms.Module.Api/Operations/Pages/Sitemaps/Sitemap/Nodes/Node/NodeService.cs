using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node
{
    /// <summary>
    /// Sitemap node service for CRUD operations.
    /// </summary>
    public class NodeService : Service, INodeService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The access control service.
        /// </summary>
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// The security service.
        /// </summary>
        private readonly ISecurityService securityService;

        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly Module.Pages.Services.ISitemapService sitemapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        public NodeService(
            IRepository repository,
            IUnitOfWork unitOfWork,
            ICmsConfiguration cmsConfiguration,
            IAccessControlService accessControlService,
            ISecurityService securityService,
            Module.Pages.Services.ISitemapService sitemapService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.cmsConfiguration = cmsConfiguration;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
            this.sitemapService = sitemapService;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetSitemapNodeResponse</c> with sitemap node data.
        /// </returns>
        public GetSitemapNodeResponse Get(GetSitemapNodeRequest request)
        {
            var model = repository
                .AsQueryable<SitemapNode>()
                .Where(node => node.Sitemap.Id == request.SitemapId && node.Id == request.NodeId && !node.IsDeleted)
                .Select(node => new SitemapNodeModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        CreatedBy = node.CreatedByUser,
                        CreatedOn = node.CreatedOn,
                        LastModifiedBy = node.ModifiedByUser,
                        LastModifiedOn = node.ModifiedOn,

                        SitemapId = node.Sitemap.Id,
                        ParentId = node.ParentNode != null && !node.ParentNode.IsDeleted ? node.ParentNode.Id : (Guid?)null,
                        PageId = node.Page != null && !node.Page.IsDeleted ? node.Page.Id : (Guid?)null,
                        PageIsPublished = node.Page != null && !node.Page.IsDeleted && node.Page.Status == PageStatus.Published,
                        PageLanguageId = node.Page != null && !node.Page.IsDeleted && node.Page.Language != null ? node.Page.Language.Id : (Guid?)null,
                        Title = node.Page != null && node.UsePageTitleAsNodeTitle ? node.Page.Title : node.Title,
                        Url = node.Page != null ? node.Page.PageUrl : node.Url,
                        DisplayOrder = node.DisplayOrder,
                        Macro = node.Macro,

                        NodeTitle = node.Title,
                        NodeUrl = node.Url,
                        UsePageTitleAsNodeTitle = node.UsePageTitleAsNodeTitle,
                    })
                .FirstOne();

            if (request.Data.IncludeTranslations)
            {
                // TODO: implement.
                throw new NotImplementedException();
            }

            return new GetSitemapNodeResponse { Data = model };
        }

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutNodeResponse</c> with create or updated node id.
        /// </returns>
        public PutNodeResponse Put(PutNodeRequest request)
        {
            var sitemapFuture = repository.AsQueryable<Module.Pages.Models.Sitemap>(e => e.Id == request.SitemapId)
                .FetchMany(f => f.AccessRules)
                .ToFuture();

            var node =
                repository.AsQueryable<SitemapNode>(e => e.Sitemap.Id == request.SitemapId && e.Id == request.NodeId)
                    .FetchMany(n => n.Translations)
                    .ToFuture()
                    .ToList()
                    .FirstOrDefault();

            var sitemap = sitemapFuture.ToList().FirstOne();

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                accessControlService.DemandAccess(sitemap, securityService.GetCurrentPrincipal(), AccessLevel.ReadWrite);
            }

            var isNew = node == null;
            if (isNew)
            {
                node = new SitemapNode
                              {
                                  Id = request.NodeId.GetValueOrDefault()
                              };
            }
            else if (request.Data.Version > 0)
            {
                node.Version = request.Data.Version;
            }

            unitOfWork.BeginTransaction();

            sitemapService.ArchiveSitemap(sitemap.Id);

            node.Title = request.Data.Title;
            if (request.Data.PageId.GetValueOrDefault() != default(Guid))
            {
                node.Page = repository.AsProxy<PageProperties>(request.Data.PageId.GetValueOrDefault());
                node.Url = null;
                node.UrlHash = null;
            }
            else
            {
                node.Page = null;
                node.Url = request.Data.Url;
                node.UrlHash = node.Url.UrlHash();
            }

            node.ParentNode = request.Data.ParentId.GetValueOrDefault() != default(Guid)
                                  ? repository.AsProxy<SitemapNode>(request.Data.ParentId.GetValueOrDefault())
                                  : null;

            node.DisplayOrder = request.Data.DisplayOrder;
            node.Macro = request.Data.Macro;
            node.UsePageTitleAsNodeTitle = request.Data.UsePageTitleAsNodeTitle;

            repository.Save(node);
            if (request.Data.Translations != null)
            {
                // TODO: implement.
                throw new NotImplementedException();
                // node.Translations;
            }

            unitOfWork.Commit();

            // Fire events.
            if (isNew)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeCreated(node);
            }
            else
            {
                Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
            }

            Events.SitemapEvents.Instance.OnSitemapUpdated(sitemap);

            return new PutNodeResponse { Data = node.Id };
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteNodeResponse</c> with success status.
        /// </returns>
        public DeleteNodeResponse Delete(DeleteNodeRequest request)
        {
            // TODO: implement.
            throw new NotImplementedException();
            Events.SitemapEvents.Instance.OnSitemapNodeDeleted(null);
        }
    }
}