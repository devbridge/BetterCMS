namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// The CategoryService interface.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Gets the specified category.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoryResponse</c> with a category.</returns>
        GetCategoryResponse Get(GetCategoryRequest request);

        /// <summary>
        /// Replaces the category or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutCategoryResponse</c> with a category id.</returns>
        PutCategoryResponse Put(PutCategoryRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostCategoryResponse Post(PostCategoryRequest request);

        /// <summary>
        /// Deletes the specified category.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteCategoryResponse</c> with success status.</returns>
        DeleteCategoryResponse Delete(DeleteCategoryRequest request);
    }
}
