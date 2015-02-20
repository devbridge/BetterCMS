using System;
using System.Linq;

using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    /// <summary>
    /// Category node service for CRUD operations.
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
        /// The category service.
        /// </summary>
        private readonly Module.Root.Services.ICategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="categoryService">The category service.</param>
        public NodeService(
            IRepository repository,
            IUnitOfWork unitOfWork,
            ICmsConfiguration cmsConfiguration,
            IAccessControlService accessControlService,
            ISecurityService securityService,
            Module.Root.Services.ICategoryService categoryService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.cmsConfiguration = cmsConfiguration;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetCategoryNodeResponse</c> with category node data.
        /// </returns>
        public GetNodeResponse Get(GetNodeRequest request)
        {
//            IEnumerable<NodeTranslationModel> translationsFuture = null;
//            if (request.Data.IncludeTranslations)
//            {
//                translationsFuture =
//                    repository.AsQueryable<Module.Root.Models.CategoryNodeTranslation>()
//                        .Where(t => t.Node.Id == request.NodeId && !t.IsDeleted)
//                        .Select(
//                            t =>
//                            new NodeTranslationModel
//                                {
//                                    Id = t.Id,
//                                    Version = t.Version,
//                                    CreatedBy = t.CreatedByUser,
//                                    CreatedOn = t.CreatedOn,
//                                    LastModifiedBy = t.ModifiedByUser,
//                                    LastModifiedOn = t.ModifiedOn,
//                                    Title = t.Title,
//                                    Url = t.Url,
//                                    UsePageTitleAsNodeTitle = t.UsePageTitleAsNodeTitle,
//                                    Macro = t.Macro,
//                                    LanguageId = t.Language.Id
//                                })
//                        .ToFuture();
//            }

            var model = repository
                .AsQueryable<Module.Root.Models.Category>()
                .Where(node => node.CategoryTree.Id == request.CategoryTreeId && node.Id == request.NodeId && !node.IsDeleted)
                .Select(node => new NodeModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        CreatedBy = node.CreatedByUser,
                        CreatedOn = node.CreatedOn,
                        LastModifiedBy = node.ModifiedByUser,
                        LastModifiedOn = node.ModifiedOn,

                        CategoryTreeId = node.CategoryTree.Id,
                        ParentId = node.ParentCategory != null && !node.ParentCategory.IsDeleted ? node.ParentCategory.Id : (Guid?)null,
                        Name = node.Name,
                        DisplayOrder = node.DisplayOrder,
                        Macro = node.Macro,
                    })
                .ToFuture()
                .FirstOne();

            var response = new GetNodeResponse { Data = model };
//            if (request.Data.IncludeTranslations && translationsFuture != null)
//            {
//                response.Translations = translationsFuture.ToList();
//            }

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
            var categoryFuture = repository.AsQueryable<CategoryTree>(e => e.Id == request.CategoryTreeId)
//                .FetchMany(f => f.AccessRules)
                .ToFuture();

            var node =
                repository.AsQueryable<Module.Root.Models.Category>(e => e.CategoryTree.Id == request.CategoryTreeId && e.Id == request.Id)
//                    .FetchMany(n => n.Translations)
//                    .ToFuture()
//                    .ToList()
                    .FirstOrDefault();

            var category = categoryFuture.ToList().FirstOne();

//            if (cmsConfiguration.Security.AccessControlEnabled)
//            {
//                accessControlService.DemandAccess(category, securityService.GetCurrentPrincipal(), AccessLevel.ReadWrite);
//            }

            var isNew = node == null;
            if (isNew)
            {
                node = new Module.Root.Models.Category
                              {
                                  Id = request.Id.GetValueOrDefault()
                              };
            }
            else if (request.Data.Version > 0)
            {
                node.Version = request.Data.Version;
            }

            unitOfWork.BeginTransaction();

//            categoryService.ArchiveCategory(category.Id);

            node.CategoryTree = category;
            node.Name = request.Data.Name;

            node.ParentCategory = request.Data.ParentId.GetValueOrDefault() != default(Guid)
                                  ? repository.AsProxy<Module.Root.Models.Category>(request.Data.ParentId.GetValueOrDefault())
                                  : null;

            node.DisplayOrder = request.Data.DisplayOrder;
            node.Macro = request.Data.Macro;

            repository.Save(node);

            unitOfWork.Commit();

            // Fire events.
            if (isNew)
            {
                Events.RootEvents.Instance.OnCategoryCreated(node);
            }
            else
            {
                Events.RootEvents.Instance.OnCategoryUpdated(node);
            }

            Events.RootEvents.Instance.OnCategoryTreeUpdated(category);

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
            if (request.Data == null || request.CategoryTreeId.HasDefaultValue() || request.Id.HasDefaultValue())
            {
                return new DeleteNodeResponse { Data = false };
            }
            
            categoryService.DeleteCategoryNode(request.Id, request.Data.Version, request.CategoryTreeId);

            return new DeleteNodeResponse { Data = true };
        }
    }
}