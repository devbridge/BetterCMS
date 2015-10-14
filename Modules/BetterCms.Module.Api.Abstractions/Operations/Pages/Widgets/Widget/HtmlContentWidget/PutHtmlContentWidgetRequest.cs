using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    /// <summary>
    /// Request for HTML content widget update.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutHtmlContentWidgetRequest : PutRequestBase<SaveHtmlContentWidgetModel>
    {
    }
}
