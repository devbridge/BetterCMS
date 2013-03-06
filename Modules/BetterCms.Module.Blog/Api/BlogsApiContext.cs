using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Blog.Api.Events;
using BetterCms.Module.Blog.Models;

using NHibernate.Linq;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public class BlogsApiContext : DataApiContext
    {
        private static readonly BlogEvents events;

        /// <summary>
        /// Initializes the <see cref="BlogsApiContext" /> class.
        /// </summary>
        static BlogsApiContext()
        {
            events = new BlogEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogsApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="repository">The repository.</param>
        public BlogsApiContext(ILifetimeScope lifetimeScope, IRepository repository = null)
            : base(lifetimeScope, repository)
        {
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public static BlogEvents Events
        {
            get
            {
                return events;
            }
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

                var query = Repository
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

                return Repository.AsQueryable<Author>().Fetch(a => a.Image).ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
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