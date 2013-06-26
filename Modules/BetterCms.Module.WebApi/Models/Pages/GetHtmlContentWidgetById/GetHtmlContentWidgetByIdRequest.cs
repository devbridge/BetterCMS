using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetHtmlContentWidgetById
{
    [DataContract]
    public class GetHtmlContentWidgetByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the widget id.
        /// </summary>
        /// <value>
        /// The widget id.
        /// </value>
        [DataMember(Order = 10, Name = "widgetId")]
        public System.Guid WidgetId { get; set; }
    }
}