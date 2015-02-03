namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Service contract for categories.
    /// </summary>
    public interface ICategoriesService
    {
        /// <summary>
        /// Gets the categories list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoriesResponse</c> with tags list.</returns>
        GetCategoriesResponse Get(GetCategoriesRequest request);

        // NOTE: do not implement: replaces all the categories.
        // PutCategoriesResponse Put(PutCategoriesRequest request);

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostCategoryResponse</c> with a new category id.</returns>
        PostCategoryResponse Post(PostCategoryRequest request);

        // NOTE: do not implement: drops all the categories.
        // DeleteCategoriesResponse Delete(DeleteCategoriesRequest request);
    }
}