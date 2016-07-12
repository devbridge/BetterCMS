namespace BetterCms.Core.Services
{
    /// <summary>
    /// Interface designed to control redirects
    /// </summary>
    public interface IRedirectControl
    {
        /// <summary>
        /// Finds the redirect.
        /// </summary>
        /// <param name="source">The source url.</param>
        /// <returns>Destination url</returns>
        string FindRedirect(string source);
    }
}
