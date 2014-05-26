namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Service contract for sitemaps.
    /// </summary>
    public interface ISitemapsService
    {
        /// <summary>
        /// Gets the sitemaps list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetSitemapsResponse</c> with tags list.</returns>
        GetSitemapsResponse Get(GetSitemapsRequest request);

        // NOTE: do not implement: replaces all the sitemaps.
        // PutSitemapsResponse Put(PutSitemapsRequest request);

        /// <summary>
        /// Creates a new sitemap.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostSitemapResponse</c> with a new sitemap id.</returns>
        PostSitemapResponse Post(PostSitemapRequest request);

        // NOTE: do not implement: drops all the sitemaps.
        // DeleteSitemapsResponse Delete(DeleteSitemapsRequest request);
    }
}