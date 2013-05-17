using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Module.Pages.Api.Events;
using BetterCms.Module.Pages.DataContracts.Enums;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.Services;

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
        public new static PagesApiEvents Events
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
                    .Where(p => p.Page.Id == pageId)
                    .ApplyFilters(filter, p => p.Order)
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .FetchMany(c => c.Options)
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
                    .Where(p => p.Page.Id == pageId && p.Region.Id == regionId)
                    .ApplyFilters(filter, p => p.Order)
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .FetchMany(c => c.Options)
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
                    .Where(p => p.Page.Id == pageId && p.Region.RegionIdentifier == regionIdentifier)
                    .ApplyFilters(filter, p => p.Order)
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .FetchMany(c => c.Options)
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
                    .Where(c => c.Id == id)
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
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
                        .ApplyFilters(true, filter, order, orderDescending, pageNumber, itemsPerPage)
                        .FetchMany(l => l.LayoutRegions)
                        .ThenFetch(l => l.Region)
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
                    .Where(lr => lr.Layout.Id == layoutId)
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage)
                    .Fetch(lr => lr.Region)
                    .ToList();
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
                    .AsQueryable<PageProperties>()
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage);

                query = FetchChilds(query, loadChilds);

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
                    .AsQueryable<PageProperties>()
                    .Where(p => p.Id == id);

                return FetchChilds(query, loadChilds)
                    .ToList()
                    .FirstOne();
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
        /// Creates the layout.
        /// </summary>
        /// <returns>Created layout entity</returns>
        public Layout CreateLayout(string layoutPath, string name, string previewUrl = null, IEnumerable<string> regions = null)
        {
            if (!HttpHelper.VirtualPathExists(layoutPath))
            {
                var message = string.Format("Failed to create layout: layout by given path {0} doesn't exist.", layoutPath);
                Logger.Error(message);
                throw new CmsApiValidationException(message);
            }

            try
            {
                UnitOfWork.BeginTransaction();

                var layout = new Layout
                                 {
                                     LayoutPath = layoutPath,
                                     Name = name,
                                     PreviewUrl = previewUrl
                                 };
                
                // reference or create new regions by identifiers
                if (regions != null)
                {
                    layout.LayoutRegions = new List<LayoutRegion>();
                    foreach (var regionIdentifier in regions)
                    {
                        if (string.IsNullOrWhiteSpace(regionIdentifier))
                        {
                            continue;
                        }

                        var region = LoadOrCreateRegion(regionIdentifier);

                        var layoutRegion = new LayoutRegion
                                               {
                                                   Layout = layout,
                                                   Region = region
                                               };
                        layout.LayoutRegions.Add(layoutRegion);
                    }
                }

                Repository.Save(layout);
                UnitOfWork.Commit();

                return layout;
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to create layout. Path: {0}, Name: {1}, Url: {2}", layoutPath, name, previewUrl);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Creates the layout region.
        /// </summary>
        /// <param name="layoutId">The layout id.</param>
        /// <param name="regionIdentifier">The region identifier.</param>
        /// <returns>
        /// Created layout region entity
        /// </returns>
        /// <exception cref="BetterCms.Core.Exceptions.Api.CmsApiException"></exception>
        public LayoutRegion CreateLayoutRegion(Guid layoutId, string regionIdentifier)
        {
            try
            {
                var layout = Repository.AsProxy<Layout>(layoutId);
                var region = LoadOrCreateRegion(regionIdentifier);

                if (!region.Id.HasDefaultValue())
                {
                    var exists = Repository.AsQueryable<LayoutRegion>(lr => lr.Region == region && lr.Layout == layout).Any();
                    if (exists)
                    {
                        var message = string.Format("Failed to create layout region: region {0} is already assigned.", regionIdentifier);
                        var logMessage = string.Format("{0} LayoutId: {1}", message, layoutId);
                        Logger.Error(logMessage);
                        throw new CmsApiValidationException(message);
                    }
                }

                var layoutRegion = new LayoutRegion { Layout = layout, Region = region };
                Repository.Save(layoutRegion);

                UnitOfWork.Commit();

                return layoutRegion;
            }
            catch (CmsApiValidationException)
            {
                throw;
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to create layout region.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Creates the HTML content widget.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="css">The CSS.</param>
        /// <param name="javaScript">The java script.</param>
        /// <returns>
        /// Created widget entity
        /// </returns>
        /// <exception cref="BetterCms.Core.Exceptions.Api.CmsApiException"></exception>
        public HtmlContentWidget CreateHtmlContentWidget(string name, string html, Guid? categoryId = null, string css = null, string javaScript = null)
        {
            try
            {
                UnitOfWork.BeginTransaction();

                var htmlWidget = new HtmlContentWidget
                                     {
                                         Name = name,
                                         Html = html,
                                         CustomCss = css,
                                         CustomJs = javaScript,
                                         UseHtml = !string.IsNullOrWhiteSpace(html),
                                         UseCustomCss = !string.IsNullOrWhiteSpace(css),
                                         UseCustomJs = !string.IsNullOrWhiteSpace(javaScript)
                                     };

                if (categoryId.HasValue && !categoryId.Value.HasDefaultValue())
                {
                    htmlWidget.Category = Repository.AsProxy<Category>(categoryId.Value);
                }

                var service = Resolve<IContentService>();
                var widgetToSave = (HtmlContentWidget)service.SaveContentWithStatusUpdate(htmlWidget, ContentStatus.Published);

                Repository.Save(widgetToSave);
                UnitOfWork.Commit();

                // Notify
                Events.OnWidgetCreated(widgetToSave);

                return widgetToSave;
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to create HTML content widget.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Creates the server control widget.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="widgetPath">The widget path.</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="previewUrl">The preview URL.</param>
        /// <returns>
        /// Created widget entity
        /// </returns>
        /// <exception cref="CmsApiValidationException"></exception>
        /// <exception cref="BetterCms.Core.Exceptions.Api.CmsApiException"></exception>
        public ServerControlWidget CreateServerControlWidget(string name, string widgetPath, Guid? categoryId = null, string previewUrl = null)
        {
            if (!HttpHelper.VirtualPathExists(widgetPath))
            {
                var message = string.Format("Failed to create server control widget: view by given path {0} doesn't exist.", widgetPath);
                Logger.Error(message);
                throw new CmsApiValidationException(message);
            }

            try
            {
                UnitOfWork.BeginTransaction();

                var serverWidget = new ServerControlWidget
                                       {
                                           Name = name,
                                           Url = widgetPath,
                                           PreviewUrl = previewUrl
                                       };

                if (categoryId.HasValue && !categoryId.Value.HasDefaultValue())
                {
                    serverWidget.Category = Repository.AsProxy<Category>(categoryId.Value);
                }

                var service = Resolve<IContentService>();
                var widgetToSave = (ServerControlWidget)service.SaveContentWithStatusUpdate(serverWidget, ContentStatus.Published);

                Repository.Save(widgetToSave);
                UnitOfWork.Commit();

                // Notify
                Events.OnWidgetCreated(widgetToSave);

                return widgetToSave;
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to create server control widget.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Creates the content option.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// Created content option
        /// </returns>
        /// <exception cref="BetterCms.Core.Exceptions.Api.CmsApiException"></exception>
        public ContentOption CreateContentOption(Guid contentId, string key, string defaultValue, OptionType type = OptionType.Text)
        {
            try
            {
                var option = new ContentOption
                                 {
                                     Key = key,
                                     DefaultValue = defaultValue,
                                     Type = type,
                                     Content = Repository.AsProxy<Content>(contentId)
                                 };

                Repository.Save(option);
                UnitOfWork.Commit();

                return option;
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to create server content option.");
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

        /// <summary>
        /// Loads the or creates the region.
        /// </summary>
        /// <returns>region entity</returns>
        private Region LoadOrCreateRegion(string regionIdentifier)
        {
            var region = Repository
                            .AsQueryable<Region>(r => r.RegionIdentifier == regionIdentifier)
                            .FirstOrDefault();

            if (region == null)
            {
                region = new Region { RegionIdentifier = regionIdentifier };
            }

            return region;
        }
    }
}