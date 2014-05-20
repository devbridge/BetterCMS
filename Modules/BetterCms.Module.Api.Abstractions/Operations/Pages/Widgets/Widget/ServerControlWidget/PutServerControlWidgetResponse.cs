using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    /// <summary>
    /// Server control widget update response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutServerControlWidgetResponse : SaveResponseBase
    {
    }
}
