using System;
using System.ComponentModel.DataAnnotations;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Mvc.Attributes;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class ClonePageViewModel
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "ClonePage_PageTitle_RequiredMessage")]
        [StringLength(300, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "ClonePage_PageTitle_MaxLengthMessage")]
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the page permalink.
        /// </summary>
        /// <value>
        /// The page permalink.
        /// </value>
        [CustomPageUrlValidation]
        [StringLength(1000, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "ClonePage_PageUrl_MaxLengthMessage")]
        public string PageUrl { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("PageId: {0}, Version: {1}, PageTitle: {2}, PageUrl: {3}", PageId, Version, PageTitle, PageUrl);
        }
    }
}