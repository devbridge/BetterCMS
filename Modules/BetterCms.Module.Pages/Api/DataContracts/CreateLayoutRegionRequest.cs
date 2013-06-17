using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Root.Api.Attributes;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class CreateLayoutRegionRequest
    {
        /// <summary>
        /// Gets or sets the layout id.
        /// </summary>
        /// <value>
        /// The layout id.
        /// </value>
        [EmptyGuidValidation(ErrorMessage = "Layout Id must be set.")]
        public Guid LayoutId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Region Identifier must be set.")]
        public string RegionIdentifier { get; set; }
    }
}