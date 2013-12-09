using System;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    /// <summary>
    /// Template view model.
    /// </summary>
    public class TemplateViewModel
    {
        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        /// <value>
        /// The template id.
        /// </value>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the template preview image url.
        /// </summary>
        /// <value>
        /// The template preview image url.
        /// </value>
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Gets or sets the preview thumbnail URL.
        /// </summary>
        /// <value>
        /// The preview thumbnail URL.
        /// </value>
        public string PreviewThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether template is master page.
        /// </summary>
        /// <value>
        /// <c>true</c> if template is master page; otherwise, <c>false</c>.
        /// </value>
        public bool IsMasterPage { get; set; }

        /// <summary>
        /// Gets or sets the URL hash.
        /// </summary>
        /// <value>
        /// The URL hash.
        /// </value>
        public string MasterUrlHash { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether master page has circular references to current.
        /// </summary>
        /// <value>
        /// <c>true</c> if master page has circular references to current; otherwise, <c>false</c>.
        /// </value>
        public bool IsCircularToCurrent { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, IsActive: {2}", TemplateId, Title, IsActive);
        }
    }
}