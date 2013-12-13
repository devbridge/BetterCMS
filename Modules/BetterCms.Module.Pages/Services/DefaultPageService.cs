using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Services;
using BetterCms.Core.Web;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;
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

        private IDictionary<string, IPage> temporaryPageCache;

        public DefaultPageService(IRepository repository, IRedirectService redirectService, IUrlService urlService)
        {
            this.repository = repository;
            this.redirectService = redirectService;
            this.urlService = urlService;
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

            var page = repository
                .AsQueryable<PageProperties>(p => p.PageUrlHash == trimmed)
                .Fetch(p => p.Layout)
                .FirstOrDefault();

            if (page != null)
            {
                temporaryPageCache.Add(trimmed, page);
            }

            return page;
        }
        
        /// <summary>
        /// Gets a page by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="loadFull">if set to <c>true</c> load full entity with childs.</param>
        /// <returns>
        /// A page object.
        /// </returns>
        public Page GetPageById(Guid id, bool loadFull = false)
        {
            try
            {
                var query = repository.AsQueryable<PageProperties>(x => x.Id == id);
                var page = query.First();
                if (loadFull)
                {
                    page.PageTags = repository.AsQueryable<PageTag>(x => x.Page == page).Fetch(x => x.Tag).ToList();
                }

                return page;
            }
            catch (Exception inner)
            {
                throw new PageException(string.Format("Failed to get page by Id: {0}.", id), inner);
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
        /// Gets the main culture page id by given page id.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>
        /// Main culture page id
        /// </returns>
        public Guid GetMainCulturePageId(Guid pageId)
        {
            var mainPage = repository
                .AsQueryable<Root.Models.Page>()
                .Where(p => p.Id == pageId)
                .Select(p => new { MainCulturePageId = p.MainCulturePage != null ? p.MainCulturePage.Id : (Guid?)null })
                .FirstOne();

            return mainPage.MainCulturePageId.HasValue && !mainPage.MainCulturePageId.Value.HasDefaultValue() 
                ? mainPage.MainCulturePageId.Value 
                : pageId;
        }
    }
}