using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public interface ICategoryApiService
    {
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
        IList<Category> GetCategories(Expression<Func<Category, bool>> filter = null,
            Expression<Func<Category, dynamic>> order = null,
            bool orderDescending = false,
            int? pageNumber = null,
            int? itemsPerPage = null);
    }
}
