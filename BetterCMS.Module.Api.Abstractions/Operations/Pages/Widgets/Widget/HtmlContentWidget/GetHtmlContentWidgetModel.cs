using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    [DataContract]
    public class GetHtmlContentWidgetModel
    {
        /// <summary>
        /// Gets or sets the widget id.
        /// </summary>
        /// <value>
        /// The widget id.
        /// </value>
        [DataMember]
        public System.Guid WidgetId { get; set; }
    }
}