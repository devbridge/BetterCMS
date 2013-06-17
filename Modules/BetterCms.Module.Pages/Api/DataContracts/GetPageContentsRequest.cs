using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetPageContentsRequest : GetFilteredDataRequest<PageContent>
    {
        public GetPageContentsRequest(Guid pageId, 
            Expression<Func<PageContent, bool>> filter = null, 
            Expression<Func<PageContent, dynamic>> order = null, 
            bool orderDescending = false,
            bool includeUnpublished = false,
            bool includeNotActive = false)
            : base(filter, order, orderDescending)
        {
            PageId = pageId;
            IncludeUnpublished = includeUnpublished;
            IncludeNotActive = includeNotActive;
            
            SetDefaultOrder(pc => pc.Order);
        }

        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished pages; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeUnpublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include not active contents.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include not active contents; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeNotActive { get; set; }
    }
}