using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Tree;
using BetterCms.Module.Api.Operations.Root.CategorizableItems;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services.Categories.Tree;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Category service for CRUD operations.
    /// </summary>
    public class CategoryTreeService : Service, ICategoryTreeService
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
        private readonly INodesTreeService treeService;

        /// <summary>
        /// The nodes service.
        /// </summary>
        private readonly INodesService nodesService;

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
        /// The category tree service
        /// </summary>
        private readonly Module.Root.Services.Categories.Tree.ICategoryTreeService categoryTreeService;

        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The node service.
        /// </summary>
        private readonly INodeService nodeService;

        /// <summary>
        /// The categorizable items service
        /// </summary>
        private readonly ICategorizableItemsService categorizableItemsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryTreeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="treeService">The tree service.</param>
        /// <param name="nodeService">The node service.</param>
        /// <param name="nodesService">The nodes service.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="categoryService">The category service.</param>
        /// <param name="categoryTreeService">The category tree service</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="categorizableItemsService">The categorizable items service</param>
        public CategoryTreeService(
            IRepository repository,
            IUnitOfWork unitOfWork,
            INodesTreeService treeService,
            INodeService nodeService,
            INodesService nodesService,
            IAccessControlService accessControlService,
            ISecurityService securityService,
            Module.Root.Services.ICategoryService categoryService,
            Module.Root.Services.Categories.Tree.ICategoryTreeService categoryTreeService,
            ICmsConfiguration cmsConfiguration,
            ICategorizableItemsService categorizableItemsService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.treeService = treeService;
            this.nodeService = nodeService;
            this.nodesService = nodesService;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
            this.categoryService = categoryService;
            this.categoryTreeService = categoryTreeService;
            this.cmsConfiguration = cmsConfiguration;
            this.categorizableItemsService = categorizableItemsService;
        }

        /// <summary>
        /// Gets the tree.
        /// </summary>
        /// <value>
        /// The tree.
        /// </value>
        INodesTreeService ICategoryTreeService.Tree
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
        INodesService ICategoryTreeService.Nodes
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
        INodeService ICategoryTreeService.Node
        {
            get
            {
                return nodeService;
            }
        }

        /// <summary>
        /// Gets the categorizable items.
        /// </summary>
        /// <value>
        /// The categorizable items.
        /// </value>
        ICategorizableItemsService ICategoryTreeService.CategorizableItems
        {
            get
            {
                return categorizableItemsService;
            }
        }

        /// <summary>
        /// Gets the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetCategoriesResponse</c> with category data.
        /// </returns>
        public GetCategoryTreeResponse Get(GetCategoryTreeRequest request)
        {
//            var tagsFuture = repository.AsQueryable<CategoryTag>().Where(e => e.Category.Id == request.CategoryId).Select(e => e.Tag.Name).ToFuture();

            IEnumerable<Module.Root.Models.Category> nodesFuture = null;
            if (request.Data.IncludeNodes)
            {
                 nodesFuture =
                    repository.AsQueryable<Module.Root.Models.Category>()
                        .Where(node => node.CategoryTree.Id == request.CategoryTreeId && !node.IsDeleted)
                        .ToFuture();
            }

            var categorizableItemsFuture = repository.AsQueryable<CategoryTreeCategorizableItem>().Where(c => c.CategoryTree.Id == request.CategoryTreeId).ToFuture();

            var response =
                repository.AsQueryable<CategoryTree>()
                    .Where(s => s.Id == request.CategoryTreeId)
                    .Select(
                        s =>
                        new GetCategoryTreeResponse
                            {
                                Data =
                                    new CategoryTreeModel
                                        {
                                            Id = s.Id,
                                            CreatedBy = s.CreatedByUser,
                                            CreatedOn = s.CreatedOn,
                                            LastModifiedBy = s.ModifiedByUser,
                                            LastModifiedOn = s.ModifiedOn,
                                            Version = s.Version,
                                            Name = s.Title,
                                            Macro = s.Macro
                                        }
                            })
                    .ToFuture()
                    .FirstOne();

//            response.Data.Tags = tagsFuture.ToList();

            if (request.Data.IncludeNodes && nodesFuture != null)
            {
                response.Nodes =
                    nodesFuture.ToList()
                        .Select(
                            node =>
                            new CategoryNodeModel
                                {
                                    Id = node.Id,
                                    Version = node.Version,
                                    CreatedBy = node.CreatedByUser,
                                    CreatedOn = node.CreatedOn,
                                    LastModifiedBy = node.ModifiedByUser,
                                    LastModifiedOn = node.ModifiedOn,
                                    ParentId = node.ParentCategory != null && !node.ParentCategory.IsDeleted ? node.ParentCategory.Id : (Guid?)null,
                                    Name = node.Name,
                                    DisplayOrder = node.DisplayOrder,
                                    Macro = node.Macro,
                                    CategoryTreeId = node.CategoryTree.Id
                                })
                        .Distinct()
                        .ToList();
            }

            var categorizableItems = categorizableItemsFuture.ToList();

            response.Data.AvailableFor = categorizableItems.Select(c => c.CategorizableItem.Id).ToList();
//            if (request.Data.IncludeAccessRules)
//            {
//                response.AccessRules = LoadAccessRules(response.Data.Id);
//            }

            return response;
        }

        /// <summary>
        /// Puts the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutCategoriesResponse</c> with updated category data.
        /// </returns>
        public PutCategoryTreeResponse Put(PutCategoryTreeRequest request)
        {
            var serviceRequest = new SaveCategoryTreeRequest
            {
                Id = request.Id ?? Guid.Empty, 
                Title = request.Data.Name, 
                Version = request.Data.Version,
                UseForCategorizableItems = request.Data.UseForCategorizableItems
            };
            IList<Module.Root.Services.Categories.CategoryNodeModel> rootNodes = new List<Module.Root.Services.Categories.CategoryNodeModel>();
            if (request.Data.Nodes != null)
            {
                foreach (var node in request.Data.Nodes)
                {
                    rootNodes.Add(RemapChildren(node, null));
                }
                serviceRequest.RootNodes = rootNodes;
            }

            var categoryTree = categoryTreeService.Save(serviceRequest);

            return new PutCategoryTreeResponse { Data = categoryTree.Id };
        }

        private Module.Root.Services.Categories.CategoryNodeModel RemapChildren(SaveCategoryTreeNodeModel node, Guid? parentId)
        {
            var categoryNode = new Module.Root.Services.Categories.CategoryNodeModel();
            IList<Module.Root.Services.Categories.CategoryNodeModel> childrenCategories = new List<Module.Root.Services.Categories.CategoryNodeModel>();
            categoryNode.DisplayOrder = node.DisplayOrder;
            categoryNode.Id = node.Id ?? Guid.Empty;
            categoryNode.Macro = node.Macro;
            categoryNode.Title = node.Name;
            categoryNode.Version = node.Version;

            if (node.Nodes != null)
            {
                foreach (var childNode in node.Nodes)
                {
                    childrenCategories.Add(RemapChildren(childNode, node.Id));
                }
                categoryNode.ChildNodes = childrenCategories;
            }

            return categoryNode;
        }

        /// <summary>
        /// Deletes the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteCategoriesResponse</c> with success status.
        /// </returns>
        public DeleteCategoryTreeResponse Delete(DeleteCategoryTreeRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeleteCategoryTreeResponse { Data = false };
            }

            categoryTreeService.Delete(
                new Module.Root.Services.Categories.Tree.DeleteCategoryTreeRequest
                {
                    Id = request.Id,
                    CurrentUser = securityService.GetCurrentPrincipal(),
                    Version = request.Data.Version
                });

            return new DeleteCategoryTreeResponse { Data = true };
        }
    }
}