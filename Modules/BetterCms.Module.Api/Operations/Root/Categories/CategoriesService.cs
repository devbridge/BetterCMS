using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// The categories service.
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
        /// Initializes a new instance of the <see cref="CategoriesService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="categoryService">The category service.</param>
        public CategoriesService(IRepository repository, ICategoryService categoryService)
        {
            this.repository = repository;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public GetCategoriesResponse Get(GetCategoriesRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Root.Models.Category>()
                .Select(category => new CategoryModel
                    {
                        Id = category.Id,
                        Version = category.Version,
                        CreatedBy = category.CreatedByUser,
                        CreatedOn = category.CreatedOn,
                        LastModifiedBy = category.ModifiedByUser,
                        LastModifiedOn = category.ModifiedOn,
                        CategoryTreeId = category.CategoryTree.Id,
                        Name = category.Name
                    })
                .ToDataListResponse(request);

            return new GetCategoriesResponse
                       {
                           Data = listResponse
                       };
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public PostCategoryResponse Post(PostCategoryRequest request)
        {
            var result =
                categoryService.Put(
                    new PutCategoryRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostCategoryResponse { Data = result.Data };
        }
    }
}