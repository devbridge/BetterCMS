using System;

namespace BetterCms.Module.Pages.Command.Content.InsertContent
{
    public class InsertContentToPageRequest
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The widget id.
        /// </value>
        public Guid ContentId { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        public Guid RegionId { get; set; }

        /// <summary>
        /// Gets or sets the parent page content identifier.
        /// </summary>
        /// <value>
        /// The parent page content identifier.
        /// </value>
        public Guid? ParentPageContentId { get; set; }

        /// <summary>
        /// Determines, if child regions should be included to the results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if child regions should be included to the results; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeChildRegions { get; set; }
    }
}