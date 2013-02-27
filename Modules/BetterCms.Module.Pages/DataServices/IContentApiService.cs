using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.Root.Models;

using ContentEntity = BetterCms.Module.Root.Models.Content;

namespace BetterCms.Module.Pages.DataServices
{
    public interface IContentApiService
    {
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
        IList<PageContent> GetPageContents(Guid pageId,
            Expression<Func<PageContent, bool>> filter = null,
            Expression<Func<PageContent, dynamic>> order = null, 
            bool orderDescending = false, 
            int? pageNumber = null, 
            int? itemsPerPage = null);

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
        /// <returns>Page content entities list</returns>
        IList<PageContent> GetRegionContents(Guid pageId,
            Guid regionId,
            Expression<Func<PageContent, bool>> filter = null,
            Expression<Func<PageContent, dynamic>> order = null, 
            bool orderDescending = false, 
            int? pageNumber = null, 
            int? itemsPerPage = null);

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
        IList<PageContent> GetRegionContents(Guid pageId,
            string regionIdentifier,
            Expression<Func<PageContent, bool>> filter = null,
            Expression<Func<PageContent, dynamic>> order = null, 
            bool orderDescending = false, 
            int? pageNumber = null, 
            int? itemsPerPage = null);

        /// <summary>
        /// Gets the content entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Content entity</returns>
        ContentEntity GetContent(Guid id);

        /// <summary>
        /// Gets the content of the page entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Page content entity</returns>
        PageContent GetPageContent(Guid id);

        
    }
}
