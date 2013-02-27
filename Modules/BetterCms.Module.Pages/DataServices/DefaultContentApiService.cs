using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultContentApiService : ApiServiceBase, IContentApiService
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContentApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultContentApiService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of page content entities.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// Page content entities list
        /// </returns>
        public System.Collections.Generic.IList<Root.Models.PageContent> GetPageContents(Guid pageId, System.Linq.Expressions.Expression<Func<Root.Models.PageContent, bool>> filter = null, System.Linq.Expressions.Expression<Func<Root.Models.PageContent, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Order;
                }

                return repository
                    .AsQueryable<Root.Models.PageContent>()
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .FetchMany(c => c.Options)
                    .Where(p => p.Page.Id == pageId)
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage)
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
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// Page content entities list
        /// </returns>
        public System.Collections.Generic.IList<Root.Models.PageContent> GetRegionContents(Guid pageId, Guid regionId, System.Linq.Expressions.Expression<Func<Root.Models.PageContent, bool>> filter = null, System.Linq.Expressions.Expression<Func<Root.Models.PageContent, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Order;
                }

                return repository
                    .AsQueryable<Root.Models.PageContent>()
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .FetchMany(c => c.Options)
                    .Where(p => p.Page.Id == pageId && p.Region.Id == regionId)
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage)
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
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// Page content entities list
        /// </returns>
        public System.Collections.Generic.IList<Root.Models.PageContent> GetRegionContents(Guid pageId, string regionIdentifier, System.Linq.Expressions.Expression<Func<Root.Models.PageContent, bool>> filter = null, System.Linq.Expressions.Expression<Func<Root.Models.PageContent, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Order;
                }

                return repository
                    .AsQueryable<Root.Models.PageContent>()
                    .Fetch(c => c.Content)
                    .Fetch(c => c.Region)
                    .FetchMany(c => c.Options)
                    .Where(p => p.Page.Id == pageId && p.Region.RegionIdentifier == regionIdentifier)
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage)
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
        public Root.Models.Content GetContent(Guid id)
        {
            try
            {
                return repository.First<Root.Models.Content>(id);
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
        public Root.Models.PageContent GetPageContent(Guid id)
        {
            try
            {
                return repository
                    .AsQueryable<Root.Models.PageContent>()
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
    }
}