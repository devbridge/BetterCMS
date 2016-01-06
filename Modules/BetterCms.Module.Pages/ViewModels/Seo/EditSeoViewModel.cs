using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Seo
{
    /// <summary>
    /// View model 
    /// </summary>
    public class EditSeoViewModel : IAccessSecuredViewModel
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditSeo_PageTitle_RequiredMessage")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditSeo_PageTitle_MaxLengthMessage")]
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the page URL path.
        /// </summary>
        /// <value>
        /// The page URL path.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [RegularExpression(PagesConstants.InternalUrlRegularExpression, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditSeo_PageUrlPath_InvalidMessage")]
        public string PageUrlPath { get; set; }

        /// <summary>
        /// Gets or sets the changed URL path.
        /// </summary>
        /// <value>
        /// The changed URL path.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [RegularExpression(PagesConstants.InternalUrlRegularExpression, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditSeo_PageUrlPath_InvalidMessage")]
        public string ChangedUrlPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to create permanent redirect.
        /// </summary>
        /// <value>
        /// <c>true</c> if create permanent redirect; otherwise, <c>false</c>.
        /// </value>
        public bool CreatePermanentRedirect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [update sitemap].
        /// </summary>
        /// <value>
        ///   <c>true</c> if update sitemap; otherwise, <c>false</c>.
        /// </value>
        public bool UpdateSitemap { get; set; }

        /// <summary>
        /// Gets or sets the meta title.
        /// </summary>
        /// <value>
        /// The meta title.
        /// </value>
        [AllowHtml]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditSeo_MetaTitle_MaxLengthMessage")]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords.
        /// </summary>
        /// <value>
        /// The meta keywords.
        /// </value>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description.
        /// </summary>
        /// <value>
        /// The meta description.
        /// </value>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use canonical URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use canonical URL; otherwise, <c>false</c>.
        /// </value>
        public bool UseCanonicalUrl { get; set; }

        /// <summary>
        /// Gets or sets the is in sitemap.
        /// </summary>
        /// <value>
        /// The is in sitemap.
        /// </value>
        public bool IsInSitemap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Edit SEO dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if the Edit SEO dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditSeoViewModel"/> class.
        /// </summary>
        public EditSeoViewModel()
        {
            UseCanonicalUrl = true;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("PageId: {0}, Version: {1}, PageTitle: {2}, PageUrlPath: {3}", PageId, Version, PageTitle, PageUrlPath);
        }
    }
}