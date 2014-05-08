using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    /// <summary>
    /// HTML content widget update response.
    /// </summary>
    [DataContract]
    public class PutHtmlContentWidgetResponse : ResponseBase<Guid?>
    {
    }
}
