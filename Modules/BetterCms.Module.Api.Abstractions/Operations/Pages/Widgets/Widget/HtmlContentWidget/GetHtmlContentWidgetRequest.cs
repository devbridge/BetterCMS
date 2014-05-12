using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    [Route("/widgets/html-content/{WidgetId}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetHtmlContentWidgetRequest : RequestBase<GetHtmlContentWidgetModel>, IReturn<GetHtmlContentWidgetResponse>
    {
        [DataMember]
        public System.Guid WidgetId { get; set; }
    }
}