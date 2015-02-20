using System;
using System.Linq;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;

using BetterModules.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Category nodes service.
    /// </summary>
    public class NodesService : Service, INodesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The node service.
        /// </summary>
        private readonly INodeService nodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodesService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="nodeService">The node service.</param>
        public NodesService(IRepository repository, INodeService nodeService)
        {
            this.repository = repository;
            this.nodeService = nodeService;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetCategoryNodesResponse</c> with nodes list.
        /// </returns>
        public GetCategoryNodesResponse Get(GetCategoryNodesRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var query = repository.AsQueryable<Module.Root.Models.Category>();
            if (request.CategoryTreeId.HasValue)
            {
                query = query.Where(node => node.CategoryTree.Id == request.CategoryTreeId);
            }
            query = query.Where(node => !node.CategoryTree.IsDeleted && !node.IsDeleted);

            var listResponse = query.Select(node => new CategoryNodeModel
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
                        Macro = node.Macro
                    })
                .ToDataListResponse(request);

            return new GetCategoryNodesResponse { Data = listResponse };
        }

        /// <summary>
        /// Creates a new category node.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostCategoryNodeResponse</c> with a new category node id.
        /// </returns>
        public PostCategoryNodeResponse Post(PostCategoryNodeRequest request)
        {
            var result = nodeService.Put(
                    new PutNodeRequest
                    {
                        Data = request.Data,
                        User = request.User,
                        CategoryTreeId = request.CategoryTreeId
                    });

            return new PostCategoryNodeResponse { Data = result.Data };
        }
    }
}