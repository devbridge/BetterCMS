using System;
using System.ComponentModel.DataAnnotations;
using BetterCms.Module.Navigation.Content.Resources;

namespace BetterCms.Module.Navigation.ViewModels.Sitemap
{
    /// <summary>
    /// View model for image media data.
    /// </summary>
    public class SitemapNodeViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(NavigationGlobalization), ErrorMessageResourceName = "Sitemap_Dialog_NodeTitle_RequiredMessage")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(NavigationGlobalization), ErrorMessageResourceName = "Sitemap_Dialog_NodeUrl_RequiredMessage")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>
        /// The display order.
        /// </value>
        public string DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public SitemapNodeViewModel ParentNode { get; set; }
    }
}