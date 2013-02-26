using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultPageApiService : IPageApiService
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
        /// <returns>
        /// The list of property entities
        /// </returns>
        public IList<PageProperties> GetPages(Expression<Func<PageProperties, bool>> filter = null, Expression<Func<PageProperties, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            if (order == null)
            {
                order = p => p.Title;
            }

            return repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
        }
    }
}