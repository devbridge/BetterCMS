using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Blog.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Blog.DataServices
{
    public class DefaultAuthorApiService : ApiServiceBase, IAuthorApiService
    {
        private IRepository repository { get; set; }

        public DefaultAuthorApiService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of author entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        public IList<Author> GetAuthors(Expression<Func<Author, bool>> filter = null, Expression<Func<Author, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Name;
                }

                return repository.AsQueryable<Author>().Fetch(a => a.Image).ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get authors list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}