using System;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.ViewModels.Option
{
    [Serializable]
    public class OptionTranslationViewModel : IOptionTranslation
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

        string IOptionTranslation.Value 
        { 
            get
            {
                return OptionValue;
            }
            set
            {
                OptionValue = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("OptionValue: {0}, LanguageId: {1}", OptionValue, LanguageId);
        }
    }
}