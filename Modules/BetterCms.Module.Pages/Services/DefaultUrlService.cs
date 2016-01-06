using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using BetterCms.Configuration;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Criterion;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultUrlService : IUrlService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Configuration service
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUrlService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="configuration">The configuration.</param>
        public DefaultUrlService(IUnitOfWork unitOfWork, ICmsConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        /// <summary>
        /// Adds the postfix to the specified URL.
        /// </summary>
        /// <param name="url">The page URL.</param>
        /// <param name="prefixPattern">The prefix pattern.</param>
        /// <param name="unsavedUrls">The list of not saved yet urls.</param>
        /// <returns>
        /// URL with added postfix (if such needed)
        /// </returns>
        public string AddPageUrlPostfix(string url, string prefixPattern, List<string> unsavedUrls = null)
        {
            url = (url ?? string.Empty).Trim();
            var endsWithSlash = url.EndsWith("/");
            url = url.Trim('/');
            
            prefixPattern = (prefixPattern ?? string.Empty).Trim('/');
            if (string.IsNullOrWhiteSpace(prefixPattern) || prefixPattern.IndexOf("{0}", StringComparison.OrdinalIgnoreCase) == -1)
            {
                prefixPattern = "{0}";
            }

            var fullUrl = FixUrl(string.Format(prefixPattern + "/", url).Trim('/'));

            // Check, if such record exists
            var exists = PathExistsInDb(fullUrl) 
                || (unsavedUrls != null && PathExistsInUnsavedList(fullUrl, unsavedUrls));

            if (exists)
            {
                // Load all titles
                var urlToReplace = string.Format("/" + prefixPattern + "-", url);
                var urlToSearch = string.Format("{0}%", urlToReplace);
                Page alias = null;

                var paths = unitOfWork.Session
                    .QueryOver(() => alias)
                    .Where(p => !p.IsDeleted)
                    .Where(Restrictions.InsensitiveLike(Projections.Property(() => alias.PageUrl), urlToSearch))
                    .Select(p => p.PageUrl)
                    .List<string>();

                if (unsavedUrls != null)
                {
                    unsavedUrls
                        .Where(u => u.StartsWith(urlToReplace))
                        .ToList()
                        .ForEach(paths.Add);
                }

                int maxNr = 0;
                var recheckInDb = false;
                foreach (var path in paths)
                {
                    int pathNr;
                    var intStr = path.Replace(urlToReplace, null).Split('-')[0].Trim('/');

                    if (int.TryParse(intStr, out pathNr))
                    {
                        if (pathNr > maxNr)
                        {
                            maxNr = pathNr;
                        }
                    }
                    else
                    {
                        recheckInDb = true;
                    }
                }

                if (maxNr == int.MaxValue)
                {
                    recheckInDb = true;
                }
                else
                {
                    maxNr++;
                }

                if (string.IsNullOrWhiteSpace(url))
                {
                    fullUrl = string.Format(prefixPattern, "-");
                    recheckInDb = true;
                }
                else
                {
                    fullUrl = string.Format(prefixPattern + "-{1}/", url, maxNr);
                }

                if (recheckInDb)
                {
                    return AddPageUrlPostfix(fullUrl, null);
                }
            }

            // Restore trailing slash behavior if needed.
            if (configuration.UrlMode == TrailingSlashBehaviorType.Mixed)
            {
                if (endsWithSlash)
                {
                    if (!fullUrl.Trim().EndsWith("/"))
                    {
                        fullUrl = string.Concat(fullUrl, "/");
                    }
                }
                else
                {
                    if (fullUrl.Trim().EndsWith("/"))
                    {
                        fullUrl = fullUrl.TrimEnd('/').Trim();
                    }
                }
            }
            return FixUrl(fullUrl);
        }

        /// <summary>
        /// Checks if path exists in database.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Url path.</returns>
        private bool PathExistsInDb(string url)
        {
            var exists = unitOfWork.Session
                .QueryOver<Page>()
                .Where(p => !p.IsDeleted && p.PageUrlHash == url.UrlHash())
                .Select(p => p.Id)
                .RowCount();
            return exists > 0;
        }

        /// <summary>
        /// Checks if path exists in unsaved list.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="unsavedUrls">The unsaved urls.</param>
        /// <returns>
        /// Url path.
        /// </returns>
        private bool PathExistsInUnsavedList(string url, List<string> unsavedUrls)
        {
            return unsavedUrls.Any(u => u == url);
        }

        /// <summary>
        /// Validates the internal URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>true, if url is valid for internal use (without http:// and other prefixes and any suffixes, such as ?# etc.)</returns>
        public bool ValidateInternalUrl(string url)
        {
            return Regex.IsMatch(url, PagesConstants.InternalUrlRegularExpression);
        }
        
        /// <summary>
        /// Validates the internal URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>true, if url is valid for internal use (without http:// and other prefixes and any suffixes, such as ?# etc.)</returns>
        public bool ValidateExternalUrl(string url)
        {
            return Regex.IsMatch(url, PagesConstants.ExternalUrlRegularExpression);
        }

        /// <summary>
        /// Fixes the URL (adds slashes in front and bottom).
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// Fixed url
        /// </returns>
        public string FixUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (url.Trim() == "/")
                {
                    return url;
                }

                if (!url.StartsWith("/"))
                {
                    url = string.Concat("/", url);
                }

                switch (configuration.UrlMode)
                {
                    case TrailingSlashBehaviorType.TrailingSlash:
                        if (!url.EndsWith("/"))
                        {
                            url = string.Concat(url, "/");
                        }
                        break;
                    case TrailingSlashBehaviorType.NoTrailingSlash:
                        if (url.EndsWith("/"))
                        {
                            url = url.TrimEnd('/');
                        }
                        break;
                }
            }
            return url;
        }

        /// <summary>
        /// Validates the URL patterns.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="message">The message.</param>
        /// <param name="validatingFieldName">Name of the validating field.</param>
        /// <returns><c>true</c> if URL is valid</returns>
        public bool ValidateUrlPatterns(string url, out string message, string validatingFieldName = null)
        {
            message = null;

            foreach (PatternElement pattern in configuration.UrlPatterns)
            {
                var options = RegexOptions.None;
                if (pattern.IgnoreCase)
                {
                    options = options | RegexOptions.IgnoreCase;
                }
                var regex = new Regex(pattern.Expression, options);
                var matched = regex.IsMatch(url);
                if (pattern.Negate)
                {
                    if (matched)
                    {
                        message = (!string.IsNullOrWhiteSpace(validatingFieldName))
                            ? string.Format(pattern.Description, validatingFieldName)
                            : string.Format(pattern.Description, PagesGlobalization.PageUrl_PatternValidation_Message_Url);
                        return false;
                    }
                }
                else
                {
                    if (!matched)
                    {
                        message = (!string.IsNullOrWhiteSpace(validatingFieldName))
                            ? string.Format(pattern.Description, validatingFieldName)
                            : string.Format(pattern.Description, PagesGlobalization.PageUrl_PatternValidation_Message_Url);
                        return false;
                    }
                }
            }

            return true;
        }
    }
}