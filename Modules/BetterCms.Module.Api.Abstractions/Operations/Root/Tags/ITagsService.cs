namespace BetterCms.Module.Api.Operations.Root.Tags
{
    /// <summary>
    /// Tags service contract for REST.
    /// </summary>
    public interface ITagsService
    {
        /// <summary>
        /// Gets the tags list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetTagsResponse</c> with tags list.</returns>
        GetTagsResponse Get(GetTagsRequest request);

        // NOTE: do not implement: replaces all the tags.
        // PutTagsResponse Put(PutTagsRequest request);

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostTagsResponse</c> with a new tag id.</returns>
        PostTagResponse Post(PostTagRequest request);

        // NOTE: do not implement: drops all the tags.
        // DeleteTagsResponse Delete(DeleteTagsRequest request);
    }
}