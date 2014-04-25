namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Tag service contract for REST.
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Gets the specified tag.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetTagResponse</c> with a tag.</returns>
        GetTagResponse Get(GetTagRequest request);

        /// <summary>
        /// Replaces the tag or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutTagResponse</c> with a tag id.</returns>
        PutTagResponse Put(PutTagRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostTagResponse Post(PostTagRequest request);

        /// <summary>
        /// Deletes the specified tag.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteTagResponse</c> with success status.</returns>
        DeleteTagResponse Delete(DeleteTagRequest request);
    }
}
