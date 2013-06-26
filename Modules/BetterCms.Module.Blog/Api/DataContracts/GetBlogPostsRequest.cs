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
        /// <param name="includeNotActive">if set to <c>true</c> include not active.</param>
        public GetBlogPostsRequest(
            Expression<Func<BlogPost, bool>> filter = null,
            Expression<Func<BlogPost, dynamic>> order = null,
            bool orderDescending = false,
            int? itemsCount = null,
            int startItemNumber = 1,
            bool includeUnpublished = false,
            bool includeNotActive = false,
            bool includeArchived = false)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            IncludeUnpublished = includeUnpublished;
            IncludeNotActive = includeNotActive;
            IncludeArchivedItems = includeArchived;

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
        /// Gets or sets a value indicating whether to include not active posts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include not active posts; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeNotActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether include archived items.
        /// </summary>
        /// <value>
        /// <c>true</c> if include archived items; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeArchivedItems { get; set; }
    }
}