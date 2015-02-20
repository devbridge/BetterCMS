using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Sitemap
{
    /// <summary>
    /// View model for sitemap node translation data.
    /// </summary>
    public class SitemapNodeTranslationViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        public Guid LanguageId { get; set; }

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
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(NavigationGlobalization), ErrorMessageResourceName = "Sitemap_Dialog_NodeTitle_RequiredMessage")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [StringLength(MaxLength.Url, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(NavigationGlobalization), ErrorMessageResourceName = "Sitemap_Dialog_NodeUrl_RequiredMessage")]
        [RegularExpression(PagesConstants.ExternalUrlRegularExpression, ErrorMessageResourceType = typeof(NavigationGlobalization), ErrorMessageResourceName = "Sitemap_Dialog_NodeUrl_InvalidSymbol")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use page title as node title.
        /// </summary>
        /// <value>
        /// <c>true</c> if use page title as node title; otherwise, <c>false</c>.
        /// </value>
        public bool UsePageTitleAsNodeTitle { get; set; }

        /// <summary>
        /// Gets or sets the macro.
        /// </summary>
        /// <value>
        /// The macro.
        /// </value>
        [StringLength(MaxLength.Text, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Macro { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Title:{2}", Id, Version, Title);
        }
    }
}