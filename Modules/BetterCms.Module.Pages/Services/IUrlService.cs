using System.Collections.Generic;

namespace BetterCms.Module.Pages.Services
{
    public interface IUrlService
    {
        /// <summary>
        /// Adds the postfix to the specified URL.
        /// </summary>
        /// <param name="url">The page URL.</param>
        /// <param name="prefixPattern">The prefix pattern.</param>
        /// <param name="unsavedUrls">The list of not saved yet urls.</param>
        /// <returns></returns>
        string AddPageUrlPostfix(string url, string prefixPattern, List<string> unsavedUrls = null);

        /// <summary>
        /// Validates the internal URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>true, if url is valid for internal use (without http:// and other prefixes and any suffixes, such as ?# etc.)</returns>
        bool ValidateInternalUrl(string url);
        
        /// <summary>
        /// Validates the external URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>true, if url is valid for external links (can contain http://, ftp://  and other prefixes and any suffixes, such as ?# etc.)</returns>
        bool ValidateExternalUrl(string url);

        /// <summary>
        /// Validates URL using URL validation patterns from cms.config.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="message">The error message.</param>
        /// <param name="validatingFieldName">Name of the validating field.</param>
        /// <returns><c>true</c> if URL is valid</returns>
        bool ValidateUrlPatterns(string url, out string message, string validatingFieldName = null);

        /// <summary>
        /// Fixes the URL (adds slashes in front and bottom).
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Fixed url</returns>
        string FixUrl(string url);
    }
}