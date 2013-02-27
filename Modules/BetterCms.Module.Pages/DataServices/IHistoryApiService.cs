using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BetterCms.Module.Pages.DataServices
{
    public interface IHistoryApiService
    {
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
        IList<Root.Models.Content> GetContentHistory(
            Guid contentId,
            Expression<Func<Root.Models.Content, bool>> filter = null,
            Expression<Func<Root.Models.Content, dynamic>> order = null,
            bool orderDescending = false);
    }
}