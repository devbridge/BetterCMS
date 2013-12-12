using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Command.Page.GetPageTranslations
{
    public class GetPageTranslationsCommandResponse
    {
        /// <summary>
        /// Gets or sets the list of page translations.
        /// </summary>
        /// <value>
        /// The list of page translations.
        /// </value>
        public virtual List<PageTranslationViewModel> Translations { get; set; }

        /// <summary>
        /// Gets or sets the list of cultures.
        /// </summary>
        /// <value>
        /// The list of cultures.
        /// </value>
        public virtual List<LookupKeyValue> Cultures { get; set; }
    }
}