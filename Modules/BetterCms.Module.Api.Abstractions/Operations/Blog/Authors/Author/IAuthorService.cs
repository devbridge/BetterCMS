namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Contract for author service.
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Gets the specified author.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetAuthorRequest</c> with an author.</returns>
        GetAuthorResponse Get(GetAuthorRequest request);

        /// <summary>
        /// Replaces the author or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutAuthorResponse</c> with a author id.</returns>
        PutAuthorResponse Put(PutAuthorRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostAuthorResponse Post(PostAuthorRequest request);

        /// <summary>
        /// Deletes the specified author.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteAuthorResponse</c> with success status.</returns>
        DeleteAuthorResponse Delete(DeleteAuthorRequest request);
    }
}