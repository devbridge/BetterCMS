// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultUrlService.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            var endsWithSlash = url.EndsWith("/", StringComparison.Ordinal);
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
                    if (!fullUrl.Trim().EndsWith("/", StringComparison.Ordinal))
                    {
                        fullUrl = string.Concat(fullUrl, "/");
                    }
                }
                else
                {
                    if (fullUrl.Trim().EndsWith("/", StringComparison.Ordinal))
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
        /// Validates the internal URL with query string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>true, if url is valid for internal use</returns>
        public bool ValidateInternalUrlWithQueryString(string url)
        {
            return Regex.IsMatch(url, PagesConstants.InternalUrlWithQueryStringRegularExpression) && Uri.IsWellFormedUriString(url, UriKind.Relative);
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

                if (!url.StartsWith("/", StringComparison.Ordinal))
                {
                    url = string.Concat("/", url);
                }

                switch (configuration.UrlMode)
                {
                    case TrailingSlashBehaviorType.TrailingSlash:
                        if (!url.EndsWith("/", StringComparison.Ordinal))
                        {
                            url = string.Concat(url, "/");
                        }
                        break;
                    case TrailingSlashBehaviorType.NoTrailingSlash:
                        if (url.EndsWith("/", StringComparison.Ordinal))
                        {
                            url = url.TrimEnd('/');
                        }
                        break;
                }
            }
            return url;
        }

        public string FixUrlFront(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (url.Trim() == "/")
                {
                    return url;
                }
                if (!url.StartsWith("/", StringComparison.Ordinal))
                {
                    url = string.Concat("/", url);
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