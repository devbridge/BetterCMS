using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    /// <summary>
    /// Content option view model
    /// </summary>
    public class ContentOptionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentOptionViewModel" /> class.
        /// </summary>
        public ContentOptionViewModel()
        {
            Type = OptionType.Text;
        }

        /// <summary>
        /// Gets or sets the option key.
        /// </summary>
        /// <value>
        /// The option key.
        /// </value>
        [Required]
        public string OptionKey { get; set; }

        /// <summary>
        /// Gets or sets the option default value.
        /// </summary>
        /// <value>
        /// The option default value.
        /// </value>
        public string OptionDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public OptionType Type { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("OptionKey: {0}, OptionDefaultValue: {1}, Type: {2}", OptionKey, OptionDefaultValue, Type);
        }
    }
}