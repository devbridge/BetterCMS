using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.DataServices
{
    public class DefaultBlogApiService : IBlogApiService
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
            if (order == null)
            {
                order = p => p.Title;
            }

            return repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
        }
    }
}