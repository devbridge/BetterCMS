using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    /// <summary>
    /// Request for server control widget update.
    /// </summary>
    [Route("/widgets/server-control/{WidgetId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutServerControlWidgetRequest : RequestBase<SaveServerControlWidgetModel>, IReturn<PutServerControlWidgetResponse>
    {
        /// <summary>
        /// Gets or sets the widget identifier.
        /// </summary>
        /// <value>
        /// The widget identifier.
        /// </value>
        [DataMember]
        public System.Guid? WidgetId { get; set; }
    }
}
