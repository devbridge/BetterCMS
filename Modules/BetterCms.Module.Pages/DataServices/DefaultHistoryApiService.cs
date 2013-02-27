using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultHistoryApiService : ApiServiceBase, IHistoryApiService
    {
        private readonly IHistoryService historyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultHistoryApiService" /> class.
        /// </summary>
        /// <param name="historyService">The history service.</param>
        public DefaultHistoryApiService(IHistoryService historyService)
        {
            this.historyService = historyService;
        }

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
        public IList<Root.Models.Content> GetContentHistory(System.Guid contentId, System.Linq.Expressions.Expression<System.Func<Root.Models.Content, bool>> filter = null, System.Linq.Expressions.Expression<System.Func<Root.Models.Content, dynamic>> order = null, bool orderDescending = false)
        {
            try
            {
                return historyService.GetContentHistory(contentId, new SearchableGridOptions())
                    .AsQueryable()
                    .ApplyFilters(filter, order, orderDescending)
                    .ToList();
            }
            catch (System.Exception inner)
            {
                var message = string.Format("Failed to get history for content id={0}.", contentId);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}