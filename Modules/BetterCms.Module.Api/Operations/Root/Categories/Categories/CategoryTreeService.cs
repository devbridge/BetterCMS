using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Tree;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

using ServiceStack.Common;
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
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The node service.
        /// </summary>
        private readonly INodeService nodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryTreeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="treeService">The tree service.</param>
        /// <param name="nodeService">The node service.</param>
        /// <param name="nodesService">The nodes service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="categoryService">The category service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public CategoryTreeService(
            IRepository repository,
            IUnitOfWork unitOfWork,
            INodesTreeService treeService,
            INodeService nodeService,
            INodesService nodesService,
            IAccessControlService accessControlService,
            ISecurityService securityService,
            Module.Root.Services.ICategoryService categoryService,
            ICmsConfiguration cmsConfiguration)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.treeService = treeService;
            this.nodeService = nodeService;
            this.nodesService = nodesService;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
            this.categoryService = categoryService;
            this.cmsConfiguration = cmsConfiguration;
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
            IEnumerable<Module.Root.Models.Category> nodesFuture = null;
            if (request.Data.Nodes != null)
            {
                nodesFuture =
                   repository.AsQueryable<Module.Root.Models.Category>()
                       .Where(node => node.CategoryTree.Id == request.Id && !node.IsDeleted)
                       .ToFuture();
            }

            var categoryTree =
                repository.AsQueryable<CategoryTree>(e => e.Id == request.Id.GetValueOrDefault())
//                    .FetchMany(f => f.AccessRules)
                    .ToFuture()
                    .ToList()
                    .FirstOrDefault();

            var isNew = categoryTree == null;
            if (isNew)
            {
                categoryTree = new CategoryTree { Id = request.Id.GetValueOrDefault()/*, AccessRules = new List<AccessRule>()*/ };
            }
            else if (request.Data.Version > 0)
            {
                categoryTree.Version = request.Data.Version;
            }

//            if (cmsConfiguration.Security.AccessControlEnabled)
//            {
//                accessControlService.DemandAccess(category, securityService.GetCurrentPrincipal(), AccessLevel.ReadWrite);
//            }

            unitOfWork.BeginTransaction();

//            if (!isNew)
//            {
//                categoryService.ArchiveCategory(category.Id);
//            }

            categoryTree.Title = request.Data.Name;

//            IList<Tag> newTags = null;
//            if (request.Data.Tags != null)
//            {
//                tagService.SaveTags(category, request.Data.Tags, out newTags);
//            }
//
//            if (request.Data.AccessRules != null)
//            {
//                category.AccessRules.RemoveDuplicateEntities();
//                var accessRules =
//                    request.Data.AccessRules.Select(
//                        r => (IAccessRule)new AccessRule { AccessLevel = (AccessLevel)(int)r.AccessLevel, Identity = r.Identity, IsForRole = r.IsForRole })
//                        .ToList();
//                accessControlService.UpdateAccessControl(category, accessRules);
//            }

            repository.Save(categoryTree);

            var createdNodes = (IList<Module.Root.Models.Category>)new List<Module.Root.Models.Category>();
            var updatedNodes = (IList<Module.Root.Models.Category>)new List<Module.Root.Models.Category>();
            var deletedNodes = (IList<Module.Root.Models.Category>)new List<Module.Root.Models.Category>();
            if (request.Data.Nodes != null)
            {
                SaveNodes(categoryTree, request.Data.Nodes, nodesFuture != null ? nodesFuture.ToList() : new List<Module.Root.Models.Category>(), ref createdNodes, ref updatedNodes, ref deletedNodes);
            }

            unitOfWork.Commit();

            // Fire events.
//            Events.RootEvents.Instance.OnTagCreated(newTags);
            foreach (var node in createdNodes)
            {
                Events.RootEvents.Instance.OnCategoryCreated(node);
            }

            foreach (var node in updatedNodes)
            {
                Events.RootEvents.Instance.OnCategoryUpdated(node);
            }

            foreach (var node in deletedNodes)
            {
                Events.RootEvents.Instance.OnCategoryDeleted(node);
            }

            if (isNew)
            {
                Events.RootEvents.Instance.OnCategoryTreeCreated(categoryTree);
            }
            else
            {
                Events.RootEvents.Instance.OnCategoryTreeUpdated(categoryTree);
            }

            return new PutCategoryTreeResponse { Data = categoryTree.Id };
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

            categoryService.DeleteCategoryTree(request.Id, request.Data.Version, securityService.GetCurrentPrincipal());

            return new DeleteCategoryTreeResponse { Data = true };
        }

//        /// <summary>
//        /// Loads the access rules.
//        /// </summary>
//        /// <param name="categoryId">The category identifier.</param>
//        /// <returns>Page access rules collection.</returns>
//        private List<AccessRuleModel> LoadAccessRules(Guid categoryId)
//        {
//            return (from page in repository.AsQueryable<CategoryTree>()
//                    from accessRule in page.AccessRules
//                    where page.Id == categoryId
//                    orderby accessRule.IsForRole, accessRule.Identity
//                    select new AccessRuleModel
//                    {
//                        AccessLevel = (Root.AccessLevel)(int)accessRule.AccessLevel,
//                        Identity = accessRule.Identity,
//                        IsForRole = accessRule.IsForRole
//                    })
//                    .ToList();
//        }

        /// <summary>
        /// Saves the nodes.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="nodeModels">The node models.</param>
        /// <param name="currentNodes">The existing nodes.</param>
        /// <param name="createdNodes">The created nodes.</param>
        /// <param name="updatedNodes">The category nodes.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        private void SaveNodes(Module.Root.Models.CategoryTree category, IList<SaveCategoryTreeNodeModel> nodeModels, List<Module.Root.Models.Category> currentNodes, ref IList<Module.Root.Models.Category> createdNodes, ref IList<Module.Root.Models.Category> updatedNodes, ref IList<Module.Root.Models.Category> deletedNodes)
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

            SaveChildNodes(category, null, nodeModels, currentNodes, ref createdNodes, ref updatedNodes, ref deletedNodes);
        }

        /// <summary>
        /// Nodes the exist.
        /// </summary>
        /// <param name="updatedNodes">The updated nodes.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if node exists; <c>false</c> otherwise.
        /// </returns>
        private bool NodeExist(IList<SaveCategoryTreeNodeModel> updatedNodes, Guid id)
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
        /// <param name="categoryTree">The category.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodesToSave">The nodes to save.</param>
        /// <param name="currentNodes">The current nodes.</param>
        /// <param name="createdNodes">The created nodes.</param>
        /// <param name="updatedNodes">The category nodes.</param>
        /// <param name="deletedNodes">The deleted nodes.</param>
        private void SaveChildNodes(
            Module.Root.Models.CategoryTree categoryTree,
            Module.Root.Models.Category parentNode,
            IEnumerable<SaveCategoryTreeNodeModel> nodesToSave,
            IList<Module.Root.Models.Category> currentNodes,
            ref IList<Module.Root.Models.Category> createdNodes,
            ref IList<Module.Root.Models.Category> updatedNodes,
            ref IList<Module.Root.Models.Category> deletedNodes)
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
                    nodeToSave = new Module.Root.Models.Category
                                     {
                                         Id = nodeModel.Id.GetValueOrDefault(),
                                         CategoryTree = categoryTree
                                     };
                }
                else if (nodeModel.Version > 0)
                {
                    nodeToSave.Version = nodeModel.Version;
                }

                nodeToSave.Name = nodeModel.Name;
                nodeToSave.DisplayOrder = nodeModel.DisplayOrder;
                nodeToSave.Macro = nodeModel.Macro;
                nodeToSave.ParentCategory = parentNode;

                repository.Save(nodeToSave);

                if (isNew)
                {
                    createdNodes.Add(nodeToSave);
                }
                else
                {
                    updatedNodes.Add(nodeToSave);
                }

                SaveChildNodes(categoryTree, nodeToSave, nodeModel.Nodes, currentNodes, ref createdNodes, ref updatedNodes, ref deletedNodes);
            }
        }
    }
}