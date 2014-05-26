using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget.Options
{
    [DataContract]
    [Serializable]
    public class GetHtmlContentWidgetOptionsResponse : ListResponseBase<OptionModel>
    {
    }
}
