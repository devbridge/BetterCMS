using System.Collections.Generic;
using System.Web.Mvc;

namespace BetterCms.Module.Root.ViewModels.Option
{
    public class OptionValueEditViewModel : OptionViewModel
    {
        /// <summary>
        /// Gets or sets the option value.
        /// </summary>
        /// <value>
        /// The option value.
        /// </value>
        [AllowHtml]
        public string OptionValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether option is editable.
        /// </summary>
        /// <value>
        /// <c>true</c> if option is editable; otherwise, <c>false</c>.
        /// </value>
        public bool CanEditOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use default value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use default value; otherwise, <c>false</c>.
        /// </value>
        public bool UseDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the custom option value title.
        /// </summary>
        /// <value>
        /// The custom option value title.
        /// </value>
        public string CustomOptionValueTitle { get; set; }

        /// <summary>
        /// Gets or sets the value translations.
        /// </summary>
        /// <value>
        /// The value translations.
        /// </value>
        public IList<OptionTranslationViewModel> ValueTranslations { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, OptionValue: {1}", base.ToString(), OptionValue);
        }
    }
}