using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    /// <summary>
    /// HTML content widget delete request for REST.
    /// </summary>
    [Route("/widgets/html-content/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteHtmlContentWidgetRequest : DeleteRequestBase, IReturn<DeleteHtmlContentWidgetResponse>
    {
    }
}