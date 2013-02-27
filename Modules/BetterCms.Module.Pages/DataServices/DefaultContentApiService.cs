using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultContentApiService : IContentApiService
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
        public System.Collections.Generic.IList<Root.Models.PageContent> GetPageContents(System.Guid pageId, System.Linq.Expressions.Expression<System.Func<Root.Models.PageContent, bool>> filter = null, System.Linq.Expressions.Expression<System.Func<Root.Models.PageContent, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
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
        public System.Collections.Generic.IList<Root.Models.PageContent> GetRegionContents(System.Guid pageId, System.Guid regionId, System.Linq.Expressions.Expression<System.Func<Root.Models.PageContent, bool>> filter = null, System.Linq.Expressions.Expression<System.Func<Root.Models.PageContent, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
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
        public System.Collections.Generic.IList<Root.Models.PageContent> GetRegionContents(System.Guid pageId, string regionIdentifier, System.Linq.Expressions.Expression<System.Func<Root.Models.PageContent, bool>> filter = null, System.Linq.Expressions.Expression<System.Func<Root.Models.PageContent, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
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

        /// <summary>
        /// Gets the content entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Content entity
        /// </returns>
        public Root.Models.Content GetContent(System.Guid id)
        {
            return repository.First<Root.Models.Content>(id);
        }

        /// <summary>
        /// Gets the content of the page entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Page content entity
        /// </returns>
        public Root.Models.PageContent GetPageContent(System.Guid id)
        {
            return repository
                .AsQueryable<Root.Models.PageContent>()
                .Fetch(c => c.Content)
                .Fetch(c => c.Region)
                .Where(c => c.Id == id)
                .FirstOne();
        }
    }
}