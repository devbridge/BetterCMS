using BetterCms.Module.Api.Operations.Blog.Authors.Author;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    /// <summary>
    /// Author service contract for REST.
    /// </summary>
    public interface IAuthorsService
    {
        /// <summary>
        /// Gets authors list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetAuthorsResponse</c> with authors list.</returns>
        GetAuthorsResponse Get(GetAuthorsRequest request);


        // NOTE: do not implement: replaces all the authors.
        // PutTagsResponse Put(PutTagsRequest request);

        /// <summary>
        /// Creates a new author.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostAuthorsResponse</c> with a new author id.</returns>
        PostAuthorResponse Post(PostAuthorRequest request);

        // NOTE: do not implement: drops all the authors.
        // DeleteAuthorsResponse Delete(DeleteAuthorsRequest request);
    }
}