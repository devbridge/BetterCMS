using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetServerControlWidgetById
{
    [DataContract]
    public class GetServerControlWidgetByIdRequest : RequestBase
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