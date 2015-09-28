using System;
using System.Web.Mvc;

namespace BetterCms.Module.Root.ViewModels.Option
{
    [Serializable]
    public class OptionTranslationViewModel
    {
        /// <summary>
        /// Gets or sets the option key.
        /// </summary>
        /// <value>
        /// The option key.
        /// </value>
        [AllowHtml]
        public string OptionValue { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        public string LanguageId { get; set; }
    }
}