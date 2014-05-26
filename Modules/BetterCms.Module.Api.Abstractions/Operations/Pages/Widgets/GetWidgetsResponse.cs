using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    [DataContract]
    [Serializable]
    public class GetWidgetsResponse : ListResponseBase<WidgetModel>
    {
    }
}