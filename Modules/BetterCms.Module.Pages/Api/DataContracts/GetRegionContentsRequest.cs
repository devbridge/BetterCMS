using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetRegionContentsRequest : GetDataRequest<PageContent>
    {
        public GetRegionContentsRequest(Guid pageId, 
            Guid regionId, 
            Expression<Func<PageContent, bool>> filter = null, 
            Expression<Func<PageContent, dynamic>> order = null, 
            bool orderDescending = false,
            bool includeUnpublished = false,
            bool includeNotInactive = false)
            : base(filter, order, orderDescending)
        {
            PageId = pageId;
            RegionId = regionId;
            IncludeUnpublished = includeUnpublished;
            IncludeNotActive = includeNotInactive;
        }

        public GetRegionContentsRequest(Guid pageId, 
            string regionIdentifier, 
            Expression<Func<PageContent, bool>> filter = null, 
            Expression<Func<PageContent, dynamic>> order = null, 
            bool orderDescending = false,
            bool includeUnpublished = false,
            bool includeNotInactive = false)
            : base(filter, order, orderDescending)
        {
            PageId = pageId;
            RegionIdentifier = regionIdentifier;
            IncludeUnpublished = includeUnpublished;
            IncludeNotActive = includeNotInactive;
        }

        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        public Guid? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        public string RegionIdentifier { get; set; }

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