using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultTagApiService : ApiServiceBase, ITagApiService
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTagApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultTagApiService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        public System.Collections.Generic.IList<Tag> GetTags(Expression<Func<Tag, bool>> filter = null, Expression<Func<Tag, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Name;
                }

                return repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get tags list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}