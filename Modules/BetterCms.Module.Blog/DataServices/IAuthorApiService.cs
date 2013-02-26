using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.DataServices
{
    public interface IAuthorApiService
    {
        /// <summary>
        /// Gets the list of author entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of author entities
        /// </returns>
        IList<Author> GetAuthors(Expression<Func<Author, bool>> filter = null,
            Expression<Func<Author, dynamic>> order = null,
            bool orderDescending = false,
            int? pageNumber = null,
            int? itemsPerPage = null);
    }
}
