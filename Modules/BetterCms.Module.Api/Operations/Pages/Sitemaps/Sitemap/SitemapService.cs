using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

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
            var tagsFuture = repository.AsQueryable<Module.Pages.Models.SitemapTag>().Where(e => e.Sitemap.Id == request.SitemapId).Select(e => e.Tag.Name).ToFuture();

// TODO: implement.
//            var nodesFuture = 
//            if (request.Data.IncludeNodes)
//            {
//                
//            }

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

// TODO: implement.
//            if (request.Data.IncludeNodesWithTranslations)
//            {
//                response.Nodes = LoadNodes(response.Data.Id);
//            }

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
            var sitemap =
                repository.AsQueryable<Module.Pages.Models.Sitemap>(e => e.Id == request.SitemapId.GetValueOrDefault())
                    .FetchMany(f => f.AccessRules)
                    .ToList()
                    .FirstOrDefault();

            var isNew = sitemap == null;
            if (isNew)
            {
                sitemap = new Module.Pages.Models.Sitemap { Id = request.SitemapId.GetValueOrDefault(), AccessRules = new List<AccessRule>() };
            }
            else if (request.Data.Version > 0)
            {
                sitemap.Version = request.Data.Version;
            }

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                var roles = new[] { RootModuleConstants.UserRoles.EditContent };
                accessControlService.DemandAccess(sitemap, securityService.GetCurrentPrincipal(), AccessLevel.ReadWrite, roles);
            }

            unitOfWork.BeginTransaction();

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

            unitOfWork.Commit();

            // Fire events.
            Events.RootEvents.Instance.OnTagCreated(newTags);
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
            if (request.Data == null || request.Data.Id.HasDefaultValue())
            {
                return new DeleteSitemapResponse { Data = false };
            }

            sitemapService.DeleteSitemap(request.Data.Id, request.Data.Version, securityService.GetCurrentPrincipal());

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
    }
}