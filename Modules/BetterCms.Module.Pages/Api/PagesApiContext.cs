using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Module.Pages.Api.Events;
using BetterCms.Module.Pages.DataContracts.Enums;
using BetterCms.Module.Pages.Models;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using NHibernate.Linq;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public class PagesApiContext : DataApiContext
    {
        private static readonly PagesApiEvents events;

        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// The history service.
        /// </summary>
        private readonly IHistoryService historyService;

        /// <summary>
        /// Initializes the <see cref="PagesApiContext" /> class.
        /// </summary>
        static PagesApiContext()
        {
            events = new PagesApiEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagesApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="historyService">The history service.</param>
        public PagesApiContext(ILifetimeScope lifetimeScope, IRepository repository = null, ISitemapService sitemapService = null, IHistoryService historyService = null)
            : base(lifetimeScope, repository)
        {
            if (historyService == null)
            {
                this.historyService = Resolve<IHistoryService>();
            }
            else
            {
                this.historyService = historyService;
            }

            if (sitemapService == null)
            {
                this.sitemapService = Resolve<ISitemapService>();
            }
            else
            {
                this.sitemapService = sitemapService;
            }
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public static PagesApiEvents Events
        {
            get
            {
                return events;
            }
        }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        public IList<Tag> GetTags(Expression<Func<Tag, bool>> filter = null, Expression<Func<Tag, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Name;
                }
                
                return Repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get tags list.";
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of redirect entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of redirect entities
        /// </returns>
        public IList<Redirect> GetRedirects(Expression<Func<Redirect, bool>> filter = null, Expression<Func<Redirect, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.PageUrl;
                }

                return Repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get redirects list.";
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of category entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of category entities
        /// </returns>
        public IList<Category> GetCategories(Expression<Func<Category, bool>> filter = null, Expression<Func<Category, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Name;
                }

                return Repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get categories list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of page content entities.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// Page content entities list
        /// </returns>
        public IList<PageContent> GetPageContents(Guid pageId, Expression<Func<PageContent, bool>> filter = null)
        {
            try
            {
                return Repository
                    .AsQueryable<PageContent>()
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .FetchMany(c => c.Options)
                    .Where(p => p.Page.Id == pageId)
                    .ApplyFilters(filter, p => p.Order)
                    .ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get page contents by page id {0}.", pageId);
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of page region contents.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="regionId">The region id.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// Page content entities list
        /// </returns>
        public IList<PageContent> GetRegionContents(Guid pageId, Guid regionId, Expression<Func<PageContent, bool>> filter = null)
        {
            try
            {
                return Repository
                    .AsQueryable<PageContent>()
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .FetchMany(c => c.Options)
                    .Where(p => p.Page.Id == pageId && p.Region.Id == regionId)
                    .ApplyFilters(filter, p => p.Order)
                    .ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get page region contents by page id={0} and region id={1}.", pageId, regionId);
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of page region contents.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="regionIdentifier">The region identifier.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// Page content entities list
        /// </returns>
        public IList<PageContent> GetRegionContents(Guid pageId, string regionIdentifier, Expression<Func<PageContent, bool>> filter = null)
        {
            try
            {
                return Repository
                    .AsQueryable<PageContent>()
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .FetchMany(c => c.Options)
                    .Where(p => p.Page.Id == pageId && p.Region.RegionIdentifier == regionIdentifier)
                    .ApplyFilters(filter, p => p.Order)
                    .ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get page region contents by page id={0} and region identifier={1}.", pageId, regionIdentifier);
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the content entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Content entity
        /// </returns>
        public Content GetContent(Guid id)
        {
            try
            {
                return Repository.First<Content>(id);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get content by id={0}.", id);
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the content of the page entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Page content entity
        /// </returns>
        public PageContent GetPageContent(Guid id)
        {
            try
            {
                return Repository
                    .AsQueryable<PageContent>()
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .Where(c => c.Id == id)
                    .FirstOne();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get page content by id={0}.", id);
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of layout entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of layout entities
        /// </returns>
        public IList<Layout> GetLayouts(Expression<Func<Layout, bool>> filter = null, Expression<Func<Layout, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                return Repository
                        .AsQueryable<Layout>()
                        .FetchMany(l => l.LayoutRegions)
                        .ThenFetch(l => l.Region)
                        .ApplyFilters(true, filter, order, orderDescending, pageNumber, itemsPerPage)
                        .ToList();

                /* TODO: remove or use
                 * WORKS GREAT WITHOUT METHODS IN WHERE CLAUSE (SUCH AS Contains)
                 * LayoutRegion lrAlias = null;
                Region rAlias = null;

                return unitOfWork.Session
                    .QueryOver<Layout>()
                    .JoinAlias(l => l.LayoutRegions, () => lrAlias, JoinType.LeftOuterJoin)
                    .JoinAlias(l => lrAlias.Region, () => rAlias, JoinType.LeftOuterJoin)
                    .ApplySubQueryFilters(filter, order, orderDescending, pageNumber, itemsPerPage)
                    .List();
                 */
            }
            catch (Exception inner)
            {
                const string message = "Failed to get layouts list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }


        /// <summary>
        /// Gets the list of region entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of region entities
        /// </returns>
        public IList<Region> GetRegions(Expression<Func<Region, bool>> filter = null, Expression<Func<Region, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.RegionIdentifier;
                }

                return Repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get regions list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of specified layout region entities.
        /// </summary>
        /// <param name="layoutId">The layout id.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of specified layout region entities
        /// </returns>
        public IList<LayoutRegion> GetLayoutRegions(Guid layoutId, Expression<Func<LayoutRegion, bool>> filter = null, Expression<Func<LayoutRegion, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Description;
                }

                return Repository
                    .AsQueryable<LayoutRegion>()
                    .Fetch(lr => lr.Region)
                    .Where(lr => lr.Layout.Id == layoutId)
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get layout regions list for layout Id={0}.", layoutId);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of page property entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="loadChilds">Flags, which childs to load.</param>
        /// <returns>
        /// The list of property entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public IList<PageProperties> GetPages(Expression<Func<PageProperties, bool>> filter = null, Expression<Func<PageProperties, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null, PageLoadableChilds loadChilds = PageLoadableChilds.None)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Title;
                }

                var query = Repository
                    .AsQueryable<PageProperties>();

                query = FetchChilds(query, loadChilds)
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage);
                return query.ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get pages list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }


        /// <summary>
        /// Checks if page exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns>
        ///   <c>true</c>if page exists; otherwise <c>false</c>
        /// </returns>
        public bool PageExists(string pageUrl)
        {
            try
            {
                return Repository.Any<PageProperties>(p => p.PageUrl == pageUrl);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to check if page exists by url:{0}", pageUrl);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the page entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="loadChilds">Flags, which childs to load.</param>
        /// <returns>
        /// Page entity
        /// </returns>
        public PageProperties GetPage(Guid id, PageLoadableChilds loadChilds = PageLoadableChilds.All)
        {
            try
            {
                var query = Repository
                    .AsQueryable<PageProperties>();

                return FetchChilds(query, loadChilds).Where(p => p.Id == id).ToList().FirstOne();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get page exists by id:{0}", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the HTML content widget.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Html widget.</returns>
        /// <exception cref="CmsApiException">Failed to get html content widget.</exception>
        public HtmlContentWidget GetHtmlContentWidget(Guid id)
        {
            try
            {
                return Repository.First<HtmlContentWidget>(id);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get html content widget by id={0}.", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the server control widget.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Server widget.</returns>
        /// <exception cref="CmsApiException">Failed to get server control widget.</exception>
        public ServerControlWidget GetServerControlWidget(Guid id)
        {
            try
            {
                return Repository.First<ServerControlWidget>(id);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get server control widget by id={0}.", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the widgets.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> [order descending].</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>Widget list.</returns>
        /// <exception cref="CmsApiException">Failed to get widgets.</exception>
        public IList<Widget> GetWidgets(Expression<Func<Widget, bool>> filter = null, Expression<Func<Widget, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Name;
                }

                return Repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get widgets.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the page widgets.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Widget list.</returns>
        /// <exception cref="CmsApiException">Failed to get widgets.</exception>
        public IList<Widget> GetPageWidgets(Guid pageId, Expression<Func<Widget, bool>> filter = null)
        {
            try
            {
                var query = Repository
                    .AsQueryable<Widget>()
                    .Where(w => w.PageContents != null && w.PageContents.Any(c => c.Page.Id == pageId));

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return query.ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get page widgets.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
        /// <summary>
        /// Gets the list with historical content entities.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <returns>
        /// Historical content entities
        /// </returns>
        public IList<Content> GetContentHistory(Guid contentId, Expression<Func<Content, bool>> filter = null, Expression<Func<Content, dynamic>> order = null, bool orderDescending = false)
        {
            try
            {
                return historyService.GetContentHistory(contentId, new SearchableGridOptions())
                    .AsQueryable()
                    .ApplyFilters(filter, order, orderDescending)
                    .ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get history for content id={0}.", contentId);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the sitemap tree.
        /// </summary>
        /// <returns>Returns list with root nodes.</returns>
        public IList<SitemapNode> GetSitemapTree()
        {
            try
            {
                return sitemapService.GetRootNodes(string.Empty);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get sitemap tree.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns> Returns sitemap node or exception <see cref="EntityNotFoundException" />.</returns>
        /// <exception cref="EntityNotFoundException">If node was not wound.</exception>
        public SitemapNode GetNode(Guid id)
        {
            try
            {
                return Repository.First<SitemapNode>(id);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get sitemap node by id={0}.", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> [order descending].</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>Returns the list with sitemap nodes.</returns>
        public IList<SitemapNode> GetNodes(Expression<Func<SitemapNode, bool>> filter = null, Expression<Func<SitemapNode, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Title;
                }

                return Repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get sitemap nodes.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
        
        /// <summary>
        /// Fetches the child by given parameters.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="loadChilds">The load child.</param>
        /// <returns>Query with fetched children by given parameters.</returns>
        private static IQueryable<PageProperties> FetchChilds(IQueryable<PageProperties> query, PageLoadableChilds loadChilds)
        {
            if (loadChilds.HasFlag(PageLoadableChilds.LayoutRegion))
            {
                query = query
                    .Fetch(p => p.Layout)
                    .ThenFetchMany(l => l.LayoutRegions)
                    .ThenFetch(lr => lr.Region);
            }
            else if (loadChilds.HasFlag(PageLoadableChilds.Layout))
            {
                query = query.Fetch(p => p.Layout);
            }

            if (loadChilds.HasFlag(PageLoadableChilds.Category))
            {
                query = query.Fetch(p => p.Category);
            }

            if (loadChilds.HasFlag(PageLoadableChilds.Image))
            {
                query = query.Fetch(p => p.Image);
            }

            if (loadChilds.HasFlag(PageLoadableChilds.Tags))
            {
                query = query.FetchMany(p => p.PageTags).ThenFetch(pt => pt.Tag);
            }

            return query;
        }
    }
}