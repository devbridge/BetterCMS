using System.Collections.Generic;
using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Core.Services
{
    /// <summary>
    /// Defines contract to manage pages.
    /// </summary>
    public interface IPageAccessor
    {
        /// <summary>
        /// Gets current page.
        /// </summary>
        /// <returns>Current page object.</returns>
        IPage GetCurrentPage(HttpContextBase httpContext);

        /// <summary>
        /// Gets current page by given virtual path.
        /// </summary>
        /// <returns>Current page object by given virtual path.</returns>
        IPage GetPageByVirtualPath(string virtualPath);

        /// <summary>
        /// Gets the redirect.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Redirect URL</returns>
        string GetRedirect(string virtualPath);

        /// <summary>
        /// Gets the list of meta data projections.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>The list of meta data projections</returns>
        IList<IPageActionProjection> GetPageMetaData(IPage page);
    }
}
