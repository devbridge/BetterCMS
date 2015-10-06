using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    /// <summary>
    /// Request for HTML content widget creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostHtmlContentWidgetRequest : RequestBase<SaveHtmlContentWidgetModel>
    {
    }
}
