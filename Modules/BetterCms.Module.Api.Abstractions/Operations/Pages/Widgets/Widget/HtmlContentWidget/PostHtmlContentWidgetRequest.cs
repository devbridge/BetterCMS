using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    /// <summary>
    /// Request for HTML content widget creation.
    /// </summary>
    [Route("/widgets/html-content/", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostHtmlContentWidgetRequest : RequestBase<SaveHtmlContentWidgetModel>, IReturn<PostHtmlContentWidgetResponse>
    {
    }
}
