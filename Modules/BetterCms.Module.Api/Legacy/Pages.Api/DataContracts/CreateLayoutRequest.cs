using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.Pages.Mvc.Attributes;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class CreateLayoutRequest
    {
        /// <summary>
        /// Gets or sets the layout (usually .cshtml) file path.
        /// </summary>
        /// <value>
        /// The layout (usually .cshtml) file path.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Layout path is required.")]        
        [StringLength(MaxLength.Url, ErrorMessage = "Maximum length of layout path cannot exceed {1} symbols.")]
        [ValidVirtualPathValidation(ErrorMessage = "Layout by given path {0} doesn't exist.")]
        public string LayoutPath { get; set; }

        /// <summary>
        /// Gets or sets the layout name.
        /// </summary>
        /// <value>
        /// The layout name.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Layout name is required.")]
        [StringLength(MaxLength.Name, ErrorMessage = "Maximum length of layout name cannot exceed {1} symbols.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layout preview URL.
        /// </summary>
        /// <value>
        /// The layout preview URL.
        /// </value>
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of layout regions.
        /// </summary>
        /// <value>
        /// The layout regions.
        /// </value>
        public IEnumerable<string> Regions { get; set; }
    }
}