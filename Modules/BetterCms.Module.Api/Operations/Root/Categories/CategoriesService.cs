using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Default service implementation for categories CRUD.
    /// </summary>
    public class CategoriesService : Service, ICategoriesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="categoryService">The category service.</param>
        public CategoriesService(IRepository repository, ICategoryService categoryService)
        {
            this.repository = repository;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Gets the categories list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetCategoriesResponse</c> with tags list.
        /// </returns>
        public GetCategoriesResponse Get(GetCategoriesRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Module.Root.Models.CategoryTree>();

            var listResponse = query
                .Where(map => !map.IsDeleted)
                .Select(map => new CategoryModel
                {
                    Id = map.Id,
                    Version = map.Version,
                    CreatedBy = map.CreatedByUser,
                    CreatedOn = map.CreatedOn,
                    LastModifiedBy = map.ModifiedByUser,
                    LastModifiedOn = map.ModifiedOn,

                    Name = map.Title
                }).ToDataListResponse(request);

            return new GetCategoriesResponse
            {
                Data = listResponse
            };
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostCategoryResponse</c> with a new category id.
        /// </returns>
        public PostCategoryResponse Post(PostCategoryRequest request)
        {
            var result = categoryService.Put(
                    new PutCategoryRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostCategoryResponse { Data = result.Data };
        }
    }
}