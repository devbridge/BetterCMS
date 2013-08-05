using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Option;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    /// <summary>
    /// Edit basic page properties view model.
    /// </summary>
    public class EditPagePropertiesViewModel : IOptionValuesContainer
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
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PageTitle_MaxLengthMessage")]
        public string PageName { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PagePermalink_RequiredMessage")]
        [RegularExpression(PagesConstants.PageUrlRegularExpression, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PagePermalink_InvalidMessage")]
        [StringLength(MaxLength.Url, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PagePermalink_MaxLengthMessage")]
        public string PageUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the list of categories.
        /// </summary>
        /// <value>
        /// The list of categories.
        /// </value>
        public IEnumerable<LookupKeyValue> Categories { get; set; }

        /// <summary>
        /// Gets or sets the page custom CSS.
        /// </summary>
        /// <value>
        /// The page custom CSS.
        /// </value>
        public string PageCSS { get; set; }
        
        /// <summary>
        /// Gets or sets the page custom JavaScript.
        /// </summary>
        /// <value>
        /// The page custom JavaScript.
        /// </value>
        public string PageJavascript { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create permanent redirect from old URL to new URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if create permanent redirect from old URL to new URL; otherwise, <c>false</c>.
        /// </value>
        public bool RedirectFromOldUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [update sitemap].
        /// </summary>
        /// <value>
        ///   <c>true</c> if update sitemap; otherwise, <c>false</c>.
        /// </value>
        public bool UpdateSitemap { get; set; }

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
        /// Gets or sets a value indicating whether use canonical URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use canonical URL; otherwise, <c>false</c>.
        /// </value>
        public bool UseCanonicalUrl { get; set; }

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
        /// Gets or sets the image view model.
        /// </summary>
        /// <value>
        /// The image view model.
        /// </value>
        public ImageSelectorViewModel Image { get; set; }

        /// <summary>
        /// Gets or sets the secondary image.
        /// </summary>
        /// <value>
        /// The secondary image.
        /// </value>
        public ImageSelectorViewModel SecondaryImage { get; set; }

        /// <summary>
        /// Gets or sets the featured image.
        /// </summary>
        /// <value>
        /// The featured image.
        /// </value>
        public ImageSelectorViewModel FeaturedImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPagePropertiesViewModel" /> class.
        /// </summary>
        public EditPagePropertiesViewModel()
        {
            Image = new ImageSelectorViewModel();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in sitemap.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in sitemap; otherwise, <c>false</c>.
        /// </value>
        public bool IsInSitemap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is archived; otherwise, <c>false</c>.
        /// </value>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the page option values.
        /// </summary>
        /// <value>
        /// The page option values.
        /// </value>
        public IList<OptionValueViewModel> OptionValues { get; set; }

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