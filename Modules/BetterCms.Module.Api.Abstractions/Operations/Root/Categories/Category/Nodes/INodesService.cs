namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Service contract for category nodes.
    /// </summary>
    public interface INodesService
    {
        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoryNodesResponse</c> with nodes list.</returns>
        GetCategoryNodesResponse Get(GetCategoryNodesRequest request);


        // NOTE: do not implement: replaces all the category nodes.
        // PutCategoryNodesResponse Put(PutCategoryNodesRequest request);

        /// <summary>
        /// Creates a new category node.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostCategoryNodeResponse</c> with a new category node id.</returns>
        PostCategoryNodeResponse Post(PostCategoryNodeRequest request);

        // NOTE: do not implement: drops all the category nodes.
        // DeleteCategoryNodesResponse Delete(DeleteCategoryNodesRequest request);
    }
}