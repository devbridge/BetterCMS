using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public interface IPageApiService
    {
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
        IList<PageProperties> GetPages(Expression<Func<PageProperties, bool>> filter = null,
            Expression<Func<PageProperties, dynamic>> order = null,
            bool orderDescending = false,
            int? pageNumber = null,
            int? itemsPerPage = null);
    }
}
