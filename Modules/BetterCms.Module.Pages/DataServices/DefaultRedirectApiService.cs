using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultRedirectApiService : IRedirectApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRedirectApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultRedirectApiService(IRepository repository)
        {
            this.repository = repository;
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
            if (order == null)
            {
                order = p => p.PageUrl;
            }

            return repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
        }
    }
}