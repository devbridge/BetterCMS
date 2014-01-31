using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Core.Services.Caching;
using BetterCms.Core.Web;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Linq;

using Page = BetterCms.Module.Pages.Models.PageProperties;
using RootPage = BetterCms.Module.Root.Models.Page;

namespace BetterCms.Module.Pages.Services
{
    internal class DefaultPageService : IPageAccessor, IPageService
    {
        private readonly IRepository repository;
        private readonly IRedirectService redirectService;
        private readonly IUrlService urlService;
        private readonly ISecurityService securityService;
        private readonly IAccessControlService accessControlService;
        private readonly ICacheService cacheService;

        private IDictionary<string, IPage> temporaryPageCache;

        public DefaultPageService(IRepository repository, IRedirectService redirectService, IUrlService urlService,
            ISecurityService securityService, IAccessControlService accessControlService, ICacheService cacheService)
        {
            this.repository = repository;
            this.redirectService = redirectService;
            this.urlService = urlService;
            this.securityService = securityService;
            this.accessControlService = accessControlService;
            this.cacheService = cacheService;

            temporaryPageCache = new Dictionary<string, IPage>();
        }
        
        /// <summary>
        /// Gets current page.
        /// </summary>
        /// <returns>Current page object.</returns>
        public IPage GetCurrentPage(HttpContextBase httpContext)
        {
            // TODO: remove it or optimize it.
            var http = new HttpContextTool(httpContext);
            var virtualPath = HttpUtility.UrlDecode(http.GetAbsolutePath());            
            return GetPageByVirtualPath(virtualPath) ?? new Page(); // TODO: do not return empty page, should implemented another logic.
        }

        /// <summary>
        /// Gets current page by given virtual path.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns>
        /// Current page object by given virtual path.
        /// </returns>
        public IPage GetPageByVirtualPath(string virtualPath)
        {
            var trimmed = virtualPath.UrlHash();
            if (temporaryPageCache.ContainsKey(trimmed))
            {
                return temporaryPageCache[trimmed];
            }

            // NOTE: if GetPageQuery() and CachePage() is used properly below code should not be executed.
            var inSitemapFuture = repository.AsQueryable<SitemapNode>().Where(node => node.UrlHash == trimmed && !node.IsDeleted && !node.Sitemap.IsDeleted).Select(node => node.Id).ToFuture();
            var page = repository
                .AsQueryable<PageProperties>(p => p.PageUrlHash == trimmed)
                .Fetch(p => p.Layout)
                .FirstOrDefault();

            if (page != null)
            {
                page.IsInSitemap = inSitemapFuture.Any() || repository.AsQueryable<SitemapNode>().Any(node => node.Page.Id == page.Id && !node.IsDeleted && !node.Sitemap.IsDeleted);
                temporaryPageCache.Add(trimmed, page);
            }

            return page;
        }

        /// <summary>
        /// Gets the page query.
        /// </summary>
        /// <returns>Queryable to find the page.</returns>
        public IQueryable<IPage> GetPageQuery()
        {
            return repository.AsQueryable<PageProperties>();
        }

        /// <summary>
        /// Caches the page.
        /// </summary>
        /// <param name="page">The page.</param>
        public void CachePage(IPage page)
        {
            var pageProperties = page as PageProperties;
            if (pageProperties != null)
            {
                pageProperties.IsInSitemap = repository.AsQueryable<SitemapNode>().Any(node => (node.Page.Id == pageProperties.Id || node.UrlHash == pageProperties.PageUrlHash) && !node.IsDeleted && !node.Sitemap.IsDeleted);
                temporaryPageCache.Add(pageProperties.PageUrlHash, pageProperties);
            }
        }

        /// <summary>
        /// Validates the page URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="pageId">The page id.</param>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        public void ValidatePageUrl(string url, Guid? pageId = null)
        {
            // Validate url
            if (!urlService.ValidateUrl(url))
            {
                var logMessage = string.Format("Invalid page url {0}.", url);
                throw new ValidationException(() => PagesGlobalization.ValidatePageUrlCommand_InvalidUrlPath_Message, logMessage);
            }

            string patternsValidationMessage;
            if (!urlService.ValidateUrlPatterns(url, out patternsValidationMessage))
            {
                var logMessage = string.Format("{0}. URL: {1}.", patternsValidationMessage, url);
                throw new ValidationException(() => patternsValidationMessage, logMessage);
            }

            // Is Url unique
            var query = repository.AsQueryable<PageProperties>(page => page.PageUrlHash == url.UrlHash());
            if (pageId.HasValue && pageId != default(Guid))
            {
                query = query.Where(page => page.Id != pageId.Value);
            }

            if (query.Select(page => page.Id).Any())
            {
                var logMessage = string.Format("Page with entered URL {0} already exists.", url);
                throw new ValidationException(() => PagesGlobalization.ValidatePageUrlCommand_UrlAlreadyExists_Message, logMessage);
            }
        }

        /// <summary>
        /// Creates the page permalink.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="parentPageUrl">The parent page URL.</param>
        /// <returns>
        /// Created permalink
        /// </returns>
        public string CreatePagePermalink(string url, string parentPageUrl)
        {
            var prefixPattern = string.IsNullOrWhiteSpace(parentPageUrl)
                ? "{0}"
                : string.Concat(parentPageUrl.Trim('/'), "/{0}");

            url = url.Transliterate(true);
            url = urlService.AddPageUrlPostfix(url, prefixPattern);

            return url;
        }

        /// <summary>
        /// Gets the redirect for given url.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>
        /// Redirect URL
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string GetRedirect(string virtualPath)
        {
            return redirectService.GetRedirect(virtualPath);
        }

        /// <summary>
        /// Gets the list of meta data projections.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>The list of meta data projections</returns>
        public IList<IPageActionProjection> GetPageMetaData(IPage page)
        {
            var metaData = new List<IPageActionProjection>();

            var rootPage = page as RootPage;
            if (rootPage != null)
            {
                if (!string.IsNullOrWhiteSpace(rootPage.MetaDescription))
                {
                    metaData.Add(new MetaDataProjection("description", rootPage.MetaDescription));
                }
                if (!string.IsNullOrWhiteSpace(rootPage.MetaKeywords))
                {
                    metaData.Add(new MetaDataProjection("keywords", rootPage.MetaKeywords));
                }
            }

            var pageProperties = page as Page;
            if (pageProperties != null)
            {
                if (pageProperties.UseNoFollow || pageProperties.UseNoIndex)
                {
                    string robotsContent = null;
                    if (pageProperties.UseNoIndex)
                    {
                        robotsContent = "noindex";
                    }
                    if (pageProperties.UseNoFollow)
                    {
                        if (!string.IsNullOrEmpty(robotsContent))
                        {
                            robotsContent += ",";
                        }
                        robotsContent += "nofollow";
                    }
                    metaData.Add(new MetaDataProjection("robots", robotsContent));
                }
                if (pageProperties.UseCanonicalUrl)
                {
                    metaData.Add(new RelationProjection("canonical", page.PageUrl));
                }
            }
            
            return metaData;
        }

        /// <summary>
        /// Gets the list of page translation view models.
        /// </summary>
        /// <param name="languageGroupIdentifier">Language group identifier.</param>
        /// <returns>
        /// The list of page translation view models
        /// </returns>
        public IEnumerable<PageTranslationViewModel> GetPageTranslations(Guid languageGroupIdentifier)
        {
            return repository
                .AsQueryable<Root.Models.Page>()
                .Where(p => p.LanguageGroupIdentifier == languageGroupIdentifier)
                .Select(p => new PageTranslationViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    PageUrl = p.PageUrl,
                    LanguageId = p.Language.Id
                })
                .ToFuture();
        }

        /// <summary>
        /// Gets the list of denied pages ids.
        /// </summary>
        /// <param name="useCache"></param>
        /// <returns>
        /// Enumerable list of denied pages ids
        /// </returns>
        public IEnumerable<Guid> GetDeniedPages(bool useCache = true)
        {
            IEnumerable<Root.Models.Page> list;
            var principal = securityService.GetCurrentPrincipal();

            if (useCache)
            {
                var cacheKey = string.Format("CMS_DeniedPages_{0}_C9E7517250F64F84ADC8-B991C8391306", principal.Identity.Name);
                list = cacheService.Get(cacheKey, new TimeSpan(0, 0, 0, 30), LoadDeniedPages);
            }
            else
            {
                list = LoadDeniedPages();
            }

            foreach (var page in list)
            {
                var accessLevel = accessControlService.GetAccessLevel(page, principal);
                if (accessLevel == AccessLevel.Deny)
                {
                    yield return page.Id;
                }
            }
        }

        /// <summary>
        /// Loads the list of denied pages.
        /// </summary>
        /// <returns>The list of denied page</returns>
        private IEnumerable<Root.Models.Page> LoadDeniedPages()
        {
            return repository
                .AsQueryable<Root.Models.Page>()
                .Where(f => f.AccessRules.Any(b => b.AccessLevel == AccessLevel.Deny))
                .FetchMany(f => f.AccessRules)
                .ToList()
                .Distinct();
        }
    }
}