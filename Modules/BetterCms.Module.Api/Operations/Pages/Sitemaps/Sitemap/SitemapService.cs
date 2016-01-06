using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Linq;

using ServiceStack.Common;
using ServiceStack.ServiceInterface;

using AccessLevel = BetterCms.Core.Security.AccessLevel;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Sitemap service for CRUD operations.
    /// </summary>
    public class SitemapService : Service, ISitemapService
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
        /// The tree service.
        /// </summary>
        private readonly ISitemapTreeService treeService;

        /// <summary>
        /// The nodes service.
        /// </summary>
        private readonly INodesService nodesService;

        /// <summary>
        /// The tag service.
        /// </summary>
        private readonly ITagService tagService;

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
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The node service.
        /// </summary>
        private readonly INodeService nodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="treeService">The tree service.</param>
        /// <param name="nodeService">The node service.</param>
        /// <param name="nodesService">The nodes service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public SitemapService(
            IRepository repository,
            IUnitOfWork unitOfWork,
            ISitemapTreeService treeService,
            INodeService nodeService,
            INodesService nodesService,
            ITagService tagService,
            IAccessControlService accessControlService,
            ISecurityService securityService,
            Module.Pages.Services.ISitemapService sitemapService,
            ICmsConfiguration cmsConfiguration)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.treeService = treeService;
            this.nodeService = nodeService;
            this.nodesService = nodesService;
            this.tagService = tagService;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
            this.sitemapService = sitemapService;
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Gets the tree.
        /// </summary>
        /// <value>
        /// The tree.
        /// </value>
        ISitemapTreeService ISitemapService.Tree
        {
            get
            {
                return treeService;
            }
        }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        INodesService ISitemapService.Nodes
        {
            get
            {
                return nodesService;
            }
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        INodeService ISitemapService.Node
        {
            get
            {
                return nodeService;
            }
        }

        /// <summary>
        /// Gets the sitemap specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetSitemapsResponse</c> with sitemap data.
        /// </returns>
        public GetSitemapResponse Get(GetSitemapRequest request)
        {
            var tagsFuture = repository.AsQueryable<SitemapTag>().Where(e => e.Sitemap.Id == request.SitemapId).Select(e => e.Tag.Name).ToFuture();

            IEnumerable<SitemapNode> nodesFuture = null;
            if (request.Data.IncludeNodes)
            {
                 nodesFuture =
                    repository.AsQueryable<SitemapNode>()
                        .Where(node => node.Sitemap.Id == request.SitemapId && !node.IsDeleted)
                        .FetchMany(node => node.Translations)
                        .ToFuture();
            }

            var response =
                repository.AsQueryable<Module.Pages.Models.Sitemap>()
                    .Where(s => s.Id == request.SitemapId)
                    .Select(
                        s =>
                        new GetSitemapResponse
                            {
                                Data =
                                    new SitemapModel
                                        {
                                            Id = s.Id,
                                            CreatedBy = s.CreatedByUser,
                                            CreatedOn = s.CreatedOn,
                                            LastModifiedBy = s.ModifiedByUser,
                                            LastModifiedOn = s.ModifiedOn,
                                            Version = s.Version,
                                            Title = s.Title,
                                        }
                            })
                    .ToFuture()
                    .FirstOne();

            response.Data.Tags = tagsFuture.ToList();

            if (request.Data.IncludeNodes && nodesFuture != null)
            {
                response.Nodes =
                    nodesFuture.ToList()
                        .Select(
                            node =>
                            new SitemapNodeWithTranslationsModel
                                {
                                    Id = node.Id,
                                    Version = node.Version,
                                    CreatedBy = node.CreatedByUser,
                                    CreatedOn = node.CreatedOn,
                                    LastModifiedBy = node.ModifiedByUser,
                                    LastModifiedOn = node.ModifiedOn,
                                    ParentId = node.ParentNode != null && !node.ParentNode.IsDeleted ? node.ParentNode.Id : (Guid?)null,
                                    PageId = node.Page != null && !node.Page.IsDeleted ? node.Page.Id : (Guid?)null,
                                    PageIsPublished = node.Page != null && !node.Page.IsDeleted && node.Page.Status == PageStatus.Published,
                                    PageLanguageId =
                                        node.Page != null && !node.Page.IsDeleted && node.Page.Language != null
                                            ? node.Page.Language.Id
                                            : (Guid?)null,
                                    Title = node.Page != null && node.UsePageTitleAsNodeTitle ? node.Page.Title : node.Title,
                                    Url = node.Page != null ? node.Page.PageUrl : node.Url,
                                    NodeTitle = node.Title,
                                    NodeUrl = node.Url,
                                    UsePageTitleAsNodeTitle = node.UsePageTitleAsNodeTitle,
                                    DisplayOrder = node.DisplayOrder,
                                    Macro = node.Macro,
                                    Translations =
                                        node.Translations.Select(
                                            t =>
                                            new SitemapNodeTranslation
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
                                                }).ToList()
                                })
                        .Distinct()
                        .ToList();
            }

            if (request.Data.IncludeAccessRules)
            {
                response.AccessRules = LoadAccessRules(response.Data.Id);
            }

            return response;
        }

        /// <summary>
        /// Puts the sitemap specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutSitemapsResponse</c> with updated sitemap data.
        /// </returns>
        public PutSitemapResponse Put(PutSitemapRequest request)
        {
            IEnumerable<SitemapNode> nodesFuture = null;
            if (request.Data.Nodes != null)
            {
                nodesFuture =
                   repository.AsQueryable<SitemapNode>()
                       .Where(node => node.Sitemap.Id == request.Id && !node.IsDeleted)
                       .FetchMany(node => node.Translations)
                       .ToFuture();
            }

            var sitemap =
                repository.AsQueryable<Module.Pages.Models.Sitemap>(e => e.Id == request.Id.GetValueOrDefault())
                    .FetchMany(f => f.AccessRules)
                    .ToFuture()
                    .ToList()
                    .FirstOrDefault();

            var isNew = sitemap == null;
            if (isNew)
            {
                sitemap = new Module.Pages.Models.Sitemap { Id = request.Id.GetValueOrDefault(), AccessRules = new List<AccessRule>() };
            }
            else if (request.Data.Version > 0)
            {
                sitemap.Version = request.Data.Version;
            }

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                accessControlService.DemandAccess(sitemap, securityService.GetCurrentPrincipal(), AccessLevel.ReadWrite);
            }

            unitOfWork.BeginTransaction();

            if (!isNew)
            {
                sitemapService.ArchiveSitemap(sitemap.Id);
            }

            sitemap.Title = request.Data.Title;

            IList<Tag> newTags = null;
            if (request.Data.Tags != null)
            {
                tagService.SaveTags(sitemap, request.Data.Tags, out newTags);
            }

            if (request.Data.AccessRules != null)
            {
                sitemap.AccessRules.RemoveDuplicateEntities();
                var accessRules =
                    request.Data.AccessRules.Select(
                        r => (IAccessRule)new AccessRule { AccessLevel = (AccessLevel)(int)r.AccessLevel, Identity = r.Identity, IsForRole = r.IsForRole })
                        .ToList();
                accessControlService.UpdateAccessControl(sitemap, accessRules);
            }

            repository.Save(sitemap);

            var createdNodes = (IList<SitemapNode>)new List<SitemapNode>();
            var updatedNodes = (IList<SitemapNode>)new List<SitemapNode>();
            var deletedNodes = (IList<SitemapNode>)new List<SitemapNode>();
            if (request.Data.Nodes != null)
            {
                SaveNodes(sitemap, request.Data.Nodes, nodesFuture != null ? nodesFuture.ToList() : new List<SitemapNode>(), ref createdNodes, ref updatedNodes, ref deletedNodes);
            }

            unitOfWork.Commit();

            // Fire events.
            Events.RootEvents.Instance.OnTagCreated(newTags);
            foreach (var node in createdNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeCreated(node);
            }

            foreach (var node in updatedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
            }

            foreach (var node in deletedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeDeleted(node);
            }

            if (isNew)
            {
                Events.SitemapEvents.Instance.OnSitemapCreated(sitemap);
            }
            else
            {
                Events.SitemapEvents.Instance.OnSitemapUpdated(sitemap);
            }

            return new PutSitemapResponse { Data = sitemap.Id };
        }

        /// <summary>
        /// Deletes the sitemap specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteSitemapsResponse</c> with success status.
        /// </returns>
        public DeleteSitemapResponse Delete(DeleteSitemapRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeleteSitemapResponse { Data = false };
            }

            sitemapService.DeleteSitemap(request.Id, request.Data.Version, securityService.GetCurrentPrincipal());

            return new DeleteSitemapResponse { Data = true };
        }

        /// <summary>
        /// Loads the access rules.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns>Page access rules collection.</returns>
        private List<AccessRuleModel> LoadAccessRules(Guid sitemapId)
        {
            return (from page in repository.AsQueryable<Module.Pages.Models.Sitemap>()
                    from accessRule in page.AccessRules
                    where page.Id == sitemapId
                    orderby accessRule.IsForRole, accessRule.Identity
                    select new AccessRuleModel
                    {
                        AccessLevel = (Root.AccessLevel)(int)accessRule.AccessLevel,
                        Identity = accessRule.Identity,
                        IsForRole = accessRule.IsForRole
                    })
                    .ToList();
        }

        /// <summary>
        /// Saves the nodes.
        /// </summary>
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="nodeModels">The node models.</param>
        /// <param name="currentNodes">The existing nodes.</param>
        /// <param name="createdNodes">The created nodes.</param>
        /// <param name="updatedNodes">The sitemap nodes.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        private void SaveNodes(Module.Pages.Models.Sitemap sitemap, IList<SaveSitemapNodeModel> nodeModels, List<SitemapNode> currentNodes, ref IList<SitemapNode> createdNodes, ref IList<SitemapNode> updatedNodes, ref IList<SitemapNode> deletedNodes)
        {
            var removeAll = nodeModels.IsEmpty();

            foreach (var existingNode in currentNodes)
            {
                if (removeAll || !NodeExist(nodeModels, existingNode.Id))
                {
                    repository.Delete(existingNode);
                    deletedNodes.Add(existingNode);
                }
            }

            if (removeAll)
            {
                return;
            }

            SaveChildNodes(sitemap, null, nodeModels, currentNodes, ref createdNodes, ref updatedNodes, ref deletedNodes);
        }

        /// <summary>
        /// Nodes the exist.
        /// </summary>
        /// <param name="updatedNodes">The updated nodes.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if node exists; <c>false</c> otherwise.
        /// </returns>
        private bool NodeExist(IList<SaveSitemapNodeModel> updatedNodes, Guid id)
        {
            if (updatedNodes == null || updatedNodes.IsEmpty())
            {
                return false;
            }

            foreach (var node in updatedNodes)
            {
                if (node.Id == id || NodeExist(node.Nodes, id))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Saves the child nodes.
        /// </summary>
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodesToSave">The nodes to save.</param>
        /// <param name="currentNodes">The current nodes.</param>
        /// <param name="createdNodes">The created nodes.</param>
        /// <param name="updatedNodes">The sitemap nodes.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        private void SaveChildNodes(
            Module.Pages.Models.Sitemap sitemap,
            SitemapNode parentNode,
            IEnumerable<SaveSitemapNodeModel> nodesToSave,
            IList<SitemapNode> currentNodes,
            ref IList<SitemapNode> createdNodes,
            ref IList<SitemapNode> updatedNodes,
            ref IList<SitemapNode> deletedNodes)
        {
            if (nodesToSave == null)
            {
                return;
            }

            foreach (var nodeModel in nodesToSave)
            {
                var nodeToSave = currentNodes.FirstOrDefault(n => n.Id == nodeModel.Id && !n.IsDeleted);
                var isNew = nodeToSave == null;
                if (isNew)
                {
                    nodeToSave = new SitemapNode
                                     {
                                         Id = nodeModel.Id.GetValueOrDefault(),
                                         Sitemap = sitemap,
                                         Translations = new List<Module.Pages.Models.SitemapNodeTranslation>()
                                     };
                }
                else if (nodeModel.Version > 0)
                {
                    nodeToSave.Version = nodeModel.Version;
                }

                nodeToSave.Page = nodeModel.PageId.HasValue ? repository.AsProxy<PageProperties>(nodeModel.PageId.Value) : null;
                nodeToSave.UsePageTitleAsNodeTitle = nodeModel.UsePageTitleAsNodeTitle;
                nodeToSave.Title = nodeModel.Title;
                nodeToSave.Url = nodeModel.Url;
                nodeToSave.UrlHash = !string.IsNullOrWhiteSpace(nodeModel.Url) ? nodeModel.Url.UrlHash() : null;
                nodeToSave.DisplayOrder = nodeModel.DisplayOrder;
                nodeToSave.Macro = nodeModel.Macro;
                nodeToSave.ParentNode = parentNode;

                repository.Save(nodeToSave);

                SaveTranslations(nodeModel, nodeToSave);

                if (isNew)
                {
                    createdNodes.Add(nodeToSave);
                }
                else
                {
                    updatedNodes.Add(nodeToSave);
                }

                SaveChildNodes(sitemap, nodeToSave, nodeModel.Nodes, currentNodes, ref createdNodes, ref updatedNodes, ref deletedNodes);
            }
        }

        /// <summary>
        /// Saves the translations.
        /// </summary>
        /// <param name="model">The node model.</param>
        /// <param name="node">The node to save.</param>
        private void SaveTranslations(SaveSitemapNodeModel model, SitemapNode node)
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

                    translationToSave.Language = repository.AsProxy<Language>(translationModel.LanguageId);
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