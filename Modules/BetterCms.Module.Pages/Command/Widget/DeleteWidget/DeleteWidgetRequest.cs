using System;

namespace BetterCms.Module.Pages.Command.Widget.DeleteWidget
{
    public class DeleteWidgetRequest
    {
        /// <summary>
        /// Gets or sets the widget id.
        /// </summary>
        /// <value>
        /// The widget id.
        /// </value>
        public Guid WidgetId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}