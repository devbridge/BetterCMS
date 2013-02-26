using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public interface ITagApiService
    {
        /// <summary>
        /// Gets the list of tag entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        IList<Tag> GetTags(Expression<Func<Tag, bool>> filter = null,
            Expression<Func<Tag, dynamic>> order = null, 
            bool orderDescending = false, 
            int? pageNumber = null, 
            int? itemsPerPage = null);
    }
}
