using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget.Options
{
    [Route("/widgets/html-content/{WidgetId}/options", Verbs = "GET")]
    [DataContract]
    public class GetHtmlContentWidgetOptionsRequest : RequestBase<DataOptions>, IReturn<GetHtmlContentWidgetOptionsResponse>
    {
        [DataMember]
        public System.Guid WidgetId { get; set; }
    }
}
