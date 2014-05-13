using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    /// <summary>
    /// Request for HTML content widget update.
    /// </summary>
    [Route("/widgets/html-content/{WidgetId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutHtmlContentWidgetRequest : RequestBase<SaveHtmlContentWidgetModel>, IReturn<PutHtmlContentWidgetResponse>
    {
        /// <summary>
        /// Gets or sets the widget identifier.
        /// </summary>
        /// <value>
        /// The widget identifier.
        /// </value>
        [DataMember]
        public Guid? WidgetId { get; set; }
    }
}
