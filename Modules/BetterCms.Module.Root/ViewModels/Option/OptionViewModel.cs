using System.ComponentModel.DataAnnotations;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.ViewModels.Option
{
    /// <summary>
    /// Option view model
    /// </summary>
    public class OptionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionViewModel" /> class.
        /// </summary>
        public OptionViewModel()
        {
            Type = OptionType.Text;
        }

        /// <summary>
        /// Gets or sets the option key.
        /// </summary>
        /// <value>
        /// The option key.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
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