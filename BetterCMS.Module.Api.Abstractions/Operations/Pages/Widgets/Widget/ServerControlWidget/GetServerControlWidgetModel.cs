using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    [DataContract]
    public class GetServerControlWidgetModel
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