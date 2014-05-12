using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    /// <summary>
    /// HTML content widget delete request for REST.
    /// </summary>
    [Route("/widgets/html-content/{WidgetId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteHtmlContentWidgetRequest : DeleteRequestBase, IReturn<DeleteHtmlContentWidgetResponse>
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