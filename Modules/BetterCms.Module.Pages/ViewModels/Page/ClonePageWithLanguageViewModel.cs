using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class ClonePageWithLanguageViewModel : ClonePageViewModel
    {
        public ClonePageWithLanguageViewModel()
        {
            Languages = new List<LookupKeyValue>();
        }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>
        /// The language id.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "ClonePageWithLanguage_Language_RequiredMessage")]
        public Guid? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the list of languages.
        /// </summary>
        /// <value>
        /// The list of languages.
        /// </value>
        public List<LookupKeyValue> Languages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show warning about no cultures created.
        /// </summary>
        /// <value>
        /// <c>true</c> if to show warning about no cultures created; otherwise, <c>false</c>.
        /// </value>
        public bool ShowWarningAboutNoCultures { get; set; }

        /// <summary>
        /// Gets or sets the sitemap action enabled flag.
        /// </summary>
        /// <value>
        /// The sitemap action enabled flag.
        /// </value>
        public bool IsSitemapActionEnabled { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, LanguageId: {1}", base.ToString(), LanguageId);
        }
    }
}