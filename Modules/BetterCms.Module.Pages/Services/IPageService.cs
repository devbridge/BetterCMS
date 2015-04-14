using System;
using System.Collections.Generic;
using System.Security.Principal;

using BetterCms.Module.Pages.ViewModels.Page;

using BetterModules.Core.Web.Mvc;

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
        /// <param name="parentPageId">The parent page identifier.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>
        /// Created permalink
        /// </returns>
        string CreatePagePermalink(string url, string parentPageUrl, Guid? parentPageId = null, Guid? languageId = null, IEnumerable<Guid> categoryId = null);

        /// <summary>
        /// Gets the list of page translation view models.
        /// </summary>
        /// <param name="languageGroupIdentifier">Language group identifier.</param>
        /// <returns>
        /// The list of page translation view models
        /// </returns>
        IEnumerable<PageTranslationViewModel> GetPageTranslations(Guid languageGroupIdentifier);

        /// <summary>
        /// Deletes the page.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>
        /// Delete result
        /// </returns>
        bool DeletePage(DeletePageViewModel model, IPrincipal principal, IMessagesIndicator messages = null);
    }
}