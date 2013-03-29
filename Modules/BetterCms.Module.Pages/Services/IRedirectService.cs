using System;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Services
{
    public interface IRedirectService
    {
        /// <summary>
        /// Checks if such redirect doesn't exists yet and creates new redirect entity.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <returns>
        /// New created redirect entity or null, if such redirect already exists
        /// </returns>
        Redirect CreateRedirectEntity(string pageUrl, string redirectUrl);

        /// <summary>
        /// Validates the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns> true, if url is valid </returns>
        bool ValidateUrl(string url);

        /// <summary>
        /// Validates URL for hidden segments.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="invalidSegment">The invalid segment.</param>
        /// <returns>true, if url is valid</returns>
        bool ValidateUrlForHiddenSegments(string url, out string invalidSegment);

        /// <summary>
        /// Validates urls: checks if the the circular loop exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="id">The id.</param>
        void ValidateForCircularLoop(string pageUrl, string redirectUrl, Guid? id = null);

        /// <summary>
        /// Checks, if such redirect exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="id">The redirect id.</param>
        /// <returns>
        /// true, if redirect exists, false if not exists
        /// </returns>
        void ValidateRedirectExists(string pageUrl, Guid? id = null);

        /// <summary>
        /// Fixes the URL (adds slashes in front and bottom).
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Fixed url</returns>
        string FixUrl(string url);

        /// <summary>
        /// Gets the redirect.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Redirect url</returns>
        string GetRedirect(string url);

        /// <summary>
        /// Gets riderects if such redirect exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="id">The redirect Id.</param>
        /// <returns>
        /// Redirect entity or null, if such redirect doesn't already exists
        /// </returns>
        Redirect GetPageRedirect(string pageUrl, Guid? id = null);
    }
}