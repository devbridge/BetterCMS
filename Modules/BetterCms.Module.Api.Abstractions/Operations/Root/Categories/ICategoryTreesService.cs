namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Service contract for categories.
    /// </summary>
    public interface ICategoryTreesService
    {
        /// <summary>
        /// Gets the category trees list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoriesResponse</c> with tags list.</returns>
        GetCategoryTreesResponse Get(GetCategoryTreesRequest request);

        // NOTE: do not implement: replaces all the categories.
        // PutCategoriesResponse Put(PutCategoriesRequest request);

        /// <summary>
        /// Creates a new category tree.
        /// </summary>
        /// <param name="treeRequest">The request.</param>
        /// <returns><c>PostCategoryResponse</c> with a new category tree id.</returns>
        PostCategoryTreeResponse Post(PostCategoryTreeRequest treeRequest);

        // NOTE: do not implement: drops all the categories.
        // DeleteCategoriesResponse Delete(DeleteCategoriesRequest request);
    }
}