using System.ComponentModel.DataAnnotations;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Api.DataContracts.Models
{
    public class ContentOptionDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentOptionDto" /> class.
        /// </summary>
        public ContentOptionDto()
        {
            Type = OptionType.Text;
        }

        /// <summary>
        /// Gets or sets the option key.
        /// </summary>
        /// <value>
        /// The option key.
        /// </value>
        [Required(ErrorMessage = "Option Key must be set")]
        [StringLength(MaxLength.Name, ErrorMessage = "Option key length cannot exceed {1} symbols.")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the default option value.
        /// </summary>
        /// <value>
        /// The default option value.
        /// </value>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the option type.
        /// </summary>
        /// <value>
        /// The option type.
        /// </value>
        public OptionType Type { get; set; }
    }
}