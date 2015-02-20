using System.Linq;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Default service implementation for categories CRUD.
    /// </summary>
    public class CategoryTreesService : Service, ICategoryTreesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly ICategoryTreeService categoryTreeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryTreesService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="categoryTreeService">The category service.</param>
        public CategoryTreesService(IRepository repository, ICategoryTreeService categoryTreeService)
        {
            this.repository = repository;
            this.categoryTreeService = categoryTreeService;
        }

        /// <summary>
        /// Gets the categories list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetCategoriesResponse</c> with tags list.
        /// </returns>
        public GetCategoryTreesResponse Get(GetCategoryTreesRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var categorizableItemsFuture = repository.AsQueryable<CategoryTreeCategorizableItem>().ToFuture();

            var query = repository
                .AsQueryable<Module.Root.Models.CategoryTree>();

            var listResponse = query
                .Where(map => !map.IsDeleted)
                .Select(map => new CategoryTreeModel
                {
                    Id = map.Id,
                    Version = map.Version,
                    CreatedBy = map.CreatedByUser,
                    CreatedOn = map.CreatedOn,
                    LastModifiedBy = map.ModifiedByUser,
                    LastModifiedOn = map.ModifiedOn,

                    Name = map.Title,
                    Macro = map.Macro,
                }).ToDataListResponse(request);

            var categorizableItems = categorizableItemsFuture.ToList();
            foreach (var listItem in listResponse.Items)
            {
                listItem.AvailableFor = categorizableItems.Where(c => c.CategoryTree.Id == listItem.Id).Select(c => c.CategorizableItem.Id).ToList();
            }
            return new GetCategoryTreesResponse
            {
                Data = listResponse
            };
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="treeRequest">The request.</param>
        /// <returns>
        ///   <c>PostCategoryResponse</c> with a new category id.
        /// </returns>
        public PostCategoryTreeResponse Post(PostCategoryTreeRequest treeRequest)
        {
            var result = categoryTreeService.Put(
                    new PutCategoryTreeRequest
                    {
                        Data = treeRequest.Data,
                        User = treeRequest.User
                    });

            return new PostCategoryTreeResponse { Data = result.Data };
        }
    }
}