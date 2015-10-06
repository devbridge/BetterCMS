using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    /// <summary>
    /// Server control widget delete request for REST.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteServerControlWidgetRequest : DeleteRequestBase
    {
    }
}