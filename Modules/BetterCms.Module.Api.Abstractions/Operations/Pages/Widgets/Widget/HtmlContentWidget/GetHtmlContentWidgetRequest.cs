using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    [DataContract]
    [Serializable]
    public class GetHtmlContentWidgetRequest : RequestBase<GetHtmlContentWidgetModel>
    {
        [DataMember]
        public System.Guid WidgetId { get; set; }
    }
}