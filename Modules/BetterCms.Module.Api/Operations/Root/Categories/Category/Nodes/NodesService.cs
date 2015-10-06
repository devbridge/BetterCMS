using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;

using BetterModules.Core.DataAccess;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Category nodes service.
    /// </summary>
    //TODO: !!!!!!!!!!!!!!!!!!!!!
    //TODO: RENAME THIS TO "NodesController" ASAP WHEN API IS MIGRATED TO vNext, BECAUSE WEB API 2 DOESN'T WORKS WITH CONTROLLERS WITH SAME NAME IN DIFFERENT NAMESPACES
    //TODO: !!!!!!!!!!!!!!!!!!!!!
    [RoutePrefix("bcms-api")]
    public class CategoryNodesController : ApiController, INodesService
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
        /// Initializes a new instance of the <see cref="CategoryNodesController" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="nodeService">The node service.</param>
        public CategoryNodesController(IRepository repository, INodeService nodeService)
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
        [Route("categorytrees/{CategoryTreeId}/nodes/")]
        [Route("categorytrees/nodes/")]
        public GetCategoryNodesResponse Get([ModelBinder(typeof(JsonModelBinder))]GetCategoryNodesRequest request)
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
        [Route("categorytrees/{CategoryTreeId}/nodes/")]
        [UrlPopulator]
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