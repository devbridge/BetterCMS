using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    /// <summary>
    /// Server control widget creation response.
    /// </summary>
    [DataContract]
    public class PostServerControlWidgetResponse : ResponseBase<Guid?>
    {
    }
}
