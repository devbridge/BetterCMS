using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Page;

using Page = BetterCms.Module.Pages.Models.PageProperties;

namespace BetterCms.Module.Pages.Services
{
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

        /// <summary>
        /// Gets the list of page translation view models.
        /// </summary>
        /// <param name="mainPageCultureId">The main page culture id.</param>
        /// <returns>The list of page translation view models</returns>
        IEnumerable<PageTranslationViewModel> GetPageTranslations(Guid mainPageCultureId);
    }
}