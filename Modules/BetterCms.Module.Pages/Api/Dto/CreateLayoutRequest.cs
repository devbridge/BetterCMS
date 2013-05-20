using System.Collections.Generic;

namespace BetterCms.Module.Pages.Api.Dto
{
    public class CreateLayoutRequest
    {
        /// <summary>
        /// Gets or sets the layout (usually .cshtml) file path.
        /// </summary>
        /// <value>
        /// The layout (usually .cshtml) file path.
        /// </value>
        public string LayoutPath { get; set; }

        /// <summary>
        /// Gets or sets the layout name.
        /// </summary>
        /// <value>
        /// The layout name.
        /// </value>
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