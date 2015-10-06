using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    /// <summary>
    /// Request for server control widget update.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutServerControlWidgetRequest : PutRequestBase<SaveServerControlWidgetModel>
    {
    }
}
