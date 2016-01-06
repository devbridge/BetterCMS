using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
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
        public GetNodeResponse Get(GetNodeRequest request)
        {
            IEnumerable<NodeTranslationModel> translationsFuture = null;
            if (request.Data.IncludeTranslations)
            {
                translationsFuture =
                    repository.AsQueryable<Module.Pages.Models.SitemapNodeTranslation>()
                        .Where(t => t.Node.Id == request.NodeId && !t.IsDeleted)
                        .Select(
                            t =>
                            new NodeTranslationModel
                                {
                                    Id = t.Id,
                                    Version = t.Version,
                                    CreatedBy = t.CreatedByUser,
                                    CreatedOn = t.CreatedOn,
                                    LastModifiedBy = t.ModifiedByUser,
                                    LastModifiedOn = t.ModifiedOn,
                                    Title = t.Title,
                                    Url = t.Url,
                                    UsePageTitleAsNodeTitle = t.UsePageTitleAsNodeTitle,
                                    Macro = t.Macro,
                                    LanguageId = t.Language.Id
                                })
                        .ToFuture();
            }

            var model = repository
                .AsQueryable<SitemapNode>()
                .Where(node => node.Sitemap.Id == request.SitemapId && node.Id == request.NodeId && !node.IsDeleted)
                .Select(node => new NodeModel
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
                .ToFuture()
                .FirstOne();

            var response = new GetNodeResponse { Data = model };
            if (request.Data.IncludeTranslations && translationsFuture != null)
            {
                response.Translations = translationsFuture.ToList();
            }

            return response;
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
                repository.AsQueryable<SitemapNode>(e => e.Sitemap.Id == request.SitemapId && e.Id == request.Id)
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
                                  Id = request.Id.GetValueOrDefault(),
                                  Translations = new List<Module.Pages.Models.SitemapNodeTranslation>()
                              };
            }
            else if (request.Data.Version > 0)
            {
                node.Version = request.Data.Version;
            }

            unitOfWork.BeginTransaction();

            sitemapService.ArchiveSitemap(sitemap.Id);

            node.Sitemap = sitemap;
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

            UpdateTranslations(node, request.Data);

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
            if (request.Data == null || request.SitemapId.HasDefaultValue() || request.Id.HasDefaultValue())
            {
                return new DeleteNodeResponse { Data = false };
            }

            sitemapService.DeleteNode(request.Id, request.Data.Version, request.SitemapId);

            return new DeleteNodeResponse { Data = true };
        }

        /// <summary>
        /// Updates the translations.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="model">The model.</param>
        private void UpdateTranslations(SitemapNode node, SaveNodeModel model)
        {
            if (model.Translations != null)
            {
                foreach (var nodeTranslation in node.Translations)
                {
                    if (model.Translations.All(m => m.Id != nodeTranslation.Id))
                    {
                        repository.Delete(nodeTranslation);
                    }
                }

                foreach (var translationModel in model.Translations)
                {
                    var translationToSave = node.Translations.FirstOrDefault(t => t.Id == translationModel.Id);
                    var isNewTranslation = translationToSave == null;
                    if (isNewTranslation)
                    {
                        translationToSave = new Module.Pages.Models.SitemapNodeTranslation { Id = translationModel.Id.GetValueOrDefault(), Node = node };
                    }
                    else if (translationModel.Version > 0)
                    {
                        translationToSave.Version = translationModel.Version;
                    }

                    translationToSave.Language = this.repository.AsProxy<Language>(translationModel.LanguageId);
                    translationToSave.Macro = translationModel.Macro;
                    translationToSave.Title = translationModel.Title;
                    translationToSave.UsePageTitleAsNodeTitle = translationModel.UsePageTitleAsNodeTitle;
                    translationToSave.Url = translationModel.Url;
                    translationToSave.UrlHash = !string.IsNullOrWhiteSpace(translationToSave.Url) ? translationToSave.Url.UrlHash() : null;

                    if (translationToSave.Node != node)
                    {
                        translationToSave.Node = node;
                    }

                    repository.Save(translationToSave);
                }
            }
        }
    }
}