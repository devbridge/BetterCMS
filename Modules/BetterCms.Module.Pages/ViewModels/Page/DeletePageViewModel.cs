using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class DeletePageViewModel
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
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>
        /// The redirect URL.
        /// </value>
        [RegularExpression(PagesConstants.ExternalUrlRegularExpression, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "RedirectUrl_InvalidMessage")]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the security word.
        /// </summary>
        /// <value>
        /// The security word.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [RegularExpression("^[Dd][Ee][Ll][Ee][Tt][Ee]$", ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "DeletePage_EnterDelete_Message")]
        public string SecurityWord { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in sitemap.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in sitemap; otherwise, <c>false</c>.
        /// </value>
        public bool IsInSitemap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [update sitemap].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [update sitemap]; otherwise, <c>false</c>.
        /// </value>
        public bool UpdateSitemap { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        public string ValidationMessage { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("PageId: {0}, Version: {1}, RedirectUrl: {2}", PageId, Version, RedirectUrl);
        }
    }
}