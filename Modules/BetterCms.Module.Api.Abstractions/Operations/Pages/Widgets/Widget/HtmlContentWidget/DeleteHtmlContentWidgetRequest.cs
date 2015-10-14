using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    /// <summary>
    /// HTML content widget delete request for REST.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteHtmlContentWidgetRequest : DeleteRequestBase
    {
    }
}