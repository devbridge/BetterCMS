using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Content.Resources;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    /// <summary>
    /// Edit basic page properties view model.
    /// </summary>
    public class EditPagePropertiesViewModel
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        /// <value>
        /// The name of the page.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PageTitle_RequiredMessage")]
        [StringLength(300, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PageTitle_MaxLengthMessage")]
        public string PageName { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PagePermalink_RequiredMessage")]
        [RegularExpression(PagesConstants.PageUrlRegularExpression, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PagePermalink_InvalidMessage")]
        [StringLength(1000, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PagePermalink_MaxLengthMessage")]
        public string PagePermalink { get; set; }

        /// <summary>
        /// Gets or sets the featured page image URL.
        /// </summary>
        /// <value>
        /// The featured page image URL.
        /// </value>
        public string FeaturedPageImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public string FileSize { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public Guid? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public IList<string> Categories { get; set; }

        /// <summary>
        /// Gets or sets the list of authors.
        /// </summary>
        /// <value>
        /// The list of authors.
        /// </value>
        public IList<LookupKeyValue> Authors { get; set; }

        /// <summary>
        /// Gets or sets the page custom CSS.
        /// </summary>
        /// <value>
        /// The page custom CSS.
        /// </value>
        public string PageCSS { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create permanent redirect from old URL to new URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if create permanent redirect from old URL to new URL; otherwise, <c>false</c>.
        /// </value>
        public bool RedirectFromOldUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page is visible to everyone.
        /// </summary>
        /// <value>
        /// <c>true</c> if page is visible to everyone; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisibleToEveryone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page must not be scanned for links to follow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page must not be scanned for links to follow; otherwise, <c>false</c>.
        /// </value>
        public bool UseNoFollow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page must not use the index.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page must not use the index; otherwise, <c>false</c>.
        /// </value>
        public bool UseNoIndex { get; set; }

        /// <summary>
        /// Gets or sets the templates.
        /// </summary>
        /// <value>
        /// The templates.
        /// </value>
        public IList<TemplateViewModel> Templates { get; set; }

        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        /// <value>
        /// The template id.
        /// </value>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Name: {2}", Id, Version, PageName);
        }
    }
}