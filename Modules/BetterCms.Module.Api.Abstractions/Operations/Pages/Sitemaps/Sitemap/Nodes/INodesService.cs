namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    /// <summary>
    /// Service contract for sitemap nodes.
    /// </summary>
    public interface INodesService
    {
        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetSitemapNodesResponse</c> with nodes list.</returns>
        GetSitemapNodesResponse Get(GetSitemapNodesRequest request);


        // NOTE: do not implement: replaces all the sitemap nodes.
        // PutSitemapNodesResponse Put(PutSitemapNodesRequest request);

        /// <summary>
        /// Creates a new sitemap node.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostSitemapNodeResponse</c> with a new sitemap node id.</returns>
        PostSitemapNodeResponse Post(PostSitemapNodeRequest request);

        // NOTE: do not implement: drops all the sitemap nodes.
        // DeleteSitemapNodesResponse Delete(DeleteSitemapNodesRequest request);
    }
}