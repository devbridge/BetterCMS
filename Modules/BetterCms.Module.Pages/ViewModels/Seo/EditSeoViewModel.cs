using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.Pages.Content.Resources;

namespace BetterCms.Module.Pages.ViewModels.Seo
{
    /// <summary>
    /// View model 
    /// </summary>
    public class EditSeoViewModel
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
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditSeo_PageTitle_RequiredMessage")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditSeo_PageTitle_MaxLengthMessage")]
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the page URL path.
        /// </summary>
        /// <value>
        /// The page URL path.
        /// </value>
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(PagesConstants.PageUrlRegularExpression, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditSeo_PageUrlPath_InvalidMessage")]
        public string PageUrlPath { get; set; }

        /// <summary>
        /// Gets or sets the changed URL path.
        /// </summary>
        /// <value>
        /// The changed URL path.
        /// </value>
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(PagesConstants.PageUrlRegularExpression, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditSeo_PageUrlPath_InvalidMessage")]
        public string ChangedUrlPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to create permanent redirect.
        /// </summary>
        /// <value>
        /// <c>true</c> if create permanent redirect; otherwise, <c>false</c>.
        /// </value>
        public bool CreatePermanentRedirect { get; set; }

        /// <summary>
        /// Gets or sets the meta title.
        /// </summary>
        /// <value>
        /// The meta title.
        /// </value>        
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