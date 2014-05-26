using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Service contract for sitemap operations.
    /// </summary>
    public interface ISitemapService
    {
        /// <summary>
        /// Gets the tree.
        /// </summary>
        /// <value>
        /// The tree.
        /// </value>
        ISitemapTreeService Tree { get; }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        INodesService Nodes { get; }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        INodeService Node { get; }

        /// <summary>
        /// Gets the sitemap specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetSitemapsResponse</c> with sitemap data.</returns>
        GetSitemapResponse Get(GetSitemapRequest request);

        /// <summary>
        /// Puts the sitemap specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutSitemapsResponse</c> with updated sitemap data.</returns>
        PutSitemapResponse Put(PutSitemapRequest request);

        /// <summary>
        /// Deletes the sitemap specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteSitemapsResponse</c> with success status.</returns>
        DeleteSitemapResponse Delete(DeleteSitemapRequest request);
    }
}