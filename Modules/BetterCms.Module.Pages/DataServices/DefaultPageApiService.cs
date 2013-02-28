using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Pages.DataContracts.Enums;
using BetterCms.Module.Pages.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultPageApiService : ApiServiceBase, IPageApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPageApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultPageApiService(IRepository repository)
        {
            this.repository = repository;
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

                var query = repository
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
                return repository.Any<PageProperties>(p => p.PageUrl == pageUrl);
            }
            catch(Exception inner)
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
                var query = repository
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
        /// Fetches the childs by given parameters.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="loadChilds">The load childs.</param>
        /// <returns>Query wiht fetched childs by given parameters</returns>
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