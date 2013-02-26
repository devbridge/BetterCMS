using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public interface IRedirectApiService
    {
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
        IList<Redirect> GetRedirects(Expression<Func<Redirect, bool>> filter = null,
            Expression<Func<Redirect, dynamic>> order = null,
            bool orderDescending = false,
            int? pageNumber = null,
            int? itemsPerPage = null);
    }
}
