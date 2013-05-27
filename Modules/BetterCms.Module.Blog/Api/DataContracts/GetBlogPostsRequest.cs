using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Api.DataContracts
{
    public class GetBlogPostsRequest : GetDataRequest<BlogPost>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogPostsRequest" /> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <param name="startItemNumber">The start item number.</param>
        /// <param name="includeUnpublished">if set to <c>true</c> include unpublished.</param>
        /// <param name="includePrivate">if set to <c>true</c> include private.</param>
        public GetBlogPostsRequest(Expression<Func<BlogPost, bool>> filter = null, 
            Expression<Func<BlogPost, dynamic>> order = null,
            bool orderDescending = false,
            int? itemsCount = null,
            int startItemNumber = 1,
            bool includeUnpublished = false,
            bool includePrivate = false)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            IncludeUnpublished = includeUnpublished;
            IncludePrivate = includePrivate;

            SetDefaultOrder(b => b.Title);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished pages; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeUnpublished { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include private pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include private pages; otherwise, <c>false</c>.
        /// </value>
        public bool IncludePrivate { get; set; }
    }
}