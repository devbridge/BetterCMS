using BetterCms.Module.Api.Operations.Root.Categories.Category;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// The CategoriesService interface.
    /// </summary>
    public interface ICategoriesService
    {
        /// <summary>
        /// Gets categories list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoriesResponse</c> with categories list.</returns>
        GetCategoriesResponse Get(GetCategoriesRequest request);


        // NOTE: do not implement: replaces all the categories.
        // PutTagsResponse Put(PutTagsRequest request);

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostCategoriesResponse</c> with a new category id.</returns>
        PostCategoryResponse Post(PostCategoryRequest request);

        // NOTE: do not implement: drops all the categories.
        // DeleteCategoriesResponse Delete(DeleteCategoriesRequest request);
    }
}
