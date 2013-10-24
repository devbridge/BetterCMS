namespace BetterCms.Module.Pages.Command.Widget.PreviewWidget
{
    public class PreviewWidgetCommandRequest
    {
        /// <summary>
        /// Gets or sets the widget id.
        /// </summary>
        /// <value>
        /// The widget id.
        /// </value>
        public System.Guid WidgetId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether java script is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if java script is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsJavaScriptEnabled { get; set; }
    }
}