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
    public class DefaultBlogApiService : ApiServiceBase, IBlogApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBlogApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultBlogApiService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of blog entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of blog entities
        /// </returns>
        public IList<BlogPost> GetBlogPosts(Expression<Func<BlogPost, bool>> filter = null, Expression<Func<BlogPost, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Title;
                }

                var query = repository
                    .AsQueryable<BlogPost>()
                    .Fetch(b => b.Author)
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage);

                return query.ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get blog posts list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}