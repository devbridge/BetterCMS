using System;

namespace BetterCms.Module.Pages.Api.Dto
{
    public class CreateLayoutRegionRequest
    {
        /// <summary>
        /// Gets or sets the layout id.
        /// </summary>
        /// <value>
        /// The layout id.
        /// </value>
        public Guid LayoutId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        public string RegionIdentifier { get; set; }
    }
}