using System;
using System.Text.RegularExpressions;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess.DataContext;
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
        /// <returns>URL with added postfix (if such needed)</returns>
        public string AddPageUrlPostfix(string url, string prefixPattern)
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
            var exists = PathExistsInDb(fullUrl);

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
        /// Validates the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>true, if url is valid</returns>
        public bool ValidateUrl(string url)
        {
            return Regex.IsMatch(url, PagesConstants.PageUrlRegularExpression);
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