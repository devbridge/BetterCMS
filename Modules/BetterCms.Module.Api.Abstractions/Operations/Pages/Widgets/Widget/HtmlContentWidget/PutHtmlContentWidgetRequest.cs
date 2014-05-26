using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    /// <summary>
    /// Request for HTML content widget update.
    /// </summary>
    [Route("/widgets/html-content/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutHtmlContentWidgetRequest : PutRequestBase<SaveHtmlContentWidgetModel>, IReturn<PutHtmlContentWidgetResponse>
    {
    }
}
