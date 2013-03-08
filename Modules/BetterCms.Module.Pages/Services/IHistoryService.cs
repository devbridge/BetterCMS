using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.Services
{
    public interface IHistoryService
    {
        /// <summary>
        /// Gets the list with historical content entities.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="gridOptions">The grid options.</param>
        /// <returns>
        /// Historical content entities
        /// </returns>
        IList<Root.Models.Content> GetContentHistory(
            Guid contentId,
            SearchableGridOptions gridOptions);

        /// <summary>
        /// Gets the name of the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The name of the status</returns>
        string GetStatusName(ContentStatus status);
    }
}