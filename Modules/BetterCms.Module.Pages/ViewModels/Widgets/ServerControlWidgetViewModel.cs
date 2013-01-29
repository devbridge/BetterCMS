using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// Widget view model
    /// </summary>
    public class ServerControlWidgetViewModel : WidgetViewModel
    {
        /// <summary>
        /// Gets or sets the current status for the saved widget.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        public ContentStatus CurrentStatus { get; set; }

        /// <summary>
        /// Gets or sets the desirable status for the saved widget.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        public ContentStatus DesirableStatus { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        /// <value>
        /// The widget url.
        /// </value>
        [Required]
        [StringLength(MaxLength.Url)]
        public string Url { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, Url: {1}", base.ToString(), Url);
        }
    }
}