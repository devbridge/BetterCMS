using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Sitemap
{
    /// <summary>
    /// View model for sitemap node data.
    /// </summary>
    public class SitemapNodeViewModel
    {
        /// <summary>
        /// Gets or sets the sitemap identifier.
        /// </summary>
        /// <value>
        /// The sitemap identifier.
        /// </value>
        public Guid SitemapId { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the page identifier.
        /// </summary>
        /// <value>
        /// The page identifier.
        /// </value>
        public Guid PageId { get; set; }
        
        /// <summary>
        /// Gets or sets the default page translation id.
        /// </summary>
        /// <value>
        /// The default page translation identifier.
        /// </value>
        public Guid DefaultPageId { get; set; }

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
        /// Gets or sets the display order.
        /// </summary>
        /// <value>
        /// The display order.
        /// </value>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>
        /// The parent id.
        /// </value>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the child nodes.
        /// </summary>
        /// <value>
        /// The child nodes.
        /// </value>
        public IList<SitemapNodeViewModel> ChildNodes { get; set; }

        /// <summary>
        /// Gets or sets the translations.
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        public IList<SitemapNodeTranslationViewModel> Translations { get; set; }

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
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        public string PageTitle { get; set; }

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