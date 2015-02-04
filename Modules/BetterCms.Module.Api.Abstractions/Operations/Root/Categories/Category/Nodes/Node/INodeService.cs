namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    /// <summary>
    /// Service contract for category node operations.
    /// </summary>
    public interface INodeService
    {
        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoryNodeResponse</c> with category node data.</returns>
        GetNodeResponse Get(GetNodeRequest request);

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutNodeResponse</c> with create or updated node id.</returns>
        PutNodeResponse Put(PutNodeRequest request);

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteNodeResponse</c> with success status.</returns>
        DeleteNodeResponse Delete(DeleteNodeRequest request);
    }
}