using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.ViewModels.Option
{
    /// <summary>
    /// Base option view model
    /// </summary>
    [Serializable]
    public abstract class OptionViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionViewModelBase" /> class.
        /// </summary>
        public OptionViewModelBase()
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
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public OptionType Type { get; set; }

        /// <summary>
        /// Gets or sets the custom option.
        /// </summary>
        /// <value>
        /// The custom option.
        /// </value>
        public CustomOptionViewModel CustomOption { get; set; }
    }
}