using System;
using System.ComponentModel.DataAnnotations;

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
        [RegularExpression(PagesConstants.PageUrlRegularExpression)]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the security word.
        /// </summary>
        /// <value>
        /// The security word.
        /// </value>
        [Required]
        [RegularExpression("^[Dd][Ee][Ll][Ee][Tt][Ee]$")]
        public string SecurityWord { get; set; }

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