using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    [Route("/widgets/html/{WidgetId}", Verbs = "GET")]
    public class GetHtmlContentWidgetRequest : RequestBase, IReturn<GetHtmlContentWidgetResponse>
    {
        /// <summary>
        /// Gets or sets the widget id.
        /// </summary>
        /// <value>
        /// The widget id.
        /// </value>
        public System.Guid WidgetId { get; set; }
    }
}