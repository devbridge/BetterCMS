using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// Widget view model
    /// </summary>
    public class ServerControlWidgetViewModel : EditWidgetViewModel
    {
        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        /// <value>
        /// The widget url.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Url, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Url { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Url: {1}", base.ToString(), Url);
        }
    }
}