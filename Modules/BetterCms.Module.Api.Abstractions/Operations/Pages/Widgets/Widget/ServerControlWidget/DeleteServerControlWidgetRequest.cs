using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    /// <summary>
    /// Server control widget delete request for REST.
    /// </summary>
    [Route("/widgets/server-control/{WidgetId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteServerControlWidgetRequest : DeleteRequestBase, IReturn<DeleteServerControlWidgetResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid WidgetId { get; set; }
    }
}