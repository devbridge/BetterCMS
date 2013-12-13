using System;
using Page = BetterCms.Module.Pages.Models.PageProperties;

namespace BetterCms.Module.Pages.Services
{
    /// <summary>
    /// </summary>    
    public interface IPageService
    {
        /// <summary>
        /// Validates the page URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="pageId">The page id.</param>
        void ValidatePageUrl(string url, Guid? pageId = null);

        /// <summary>
        /// Creates the page permalink.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="parentPageUrl">The parent page URL.</param>
        /// <returns>
        /// Created permalink
        /// </returns>
        string CreatePagePermalink(string url, string parentPageUrl);

        /// <summary>
        /// Gets the main culture page id by given page id.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>Main culture page id</returns>
        Guid GetMainCulturePageId(Guid pageId);
    }
}