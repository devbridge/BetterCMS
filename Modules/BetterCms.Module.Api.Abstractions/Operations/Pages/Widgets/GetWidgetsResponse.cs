using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    [DataContract]
    public class GetWidgetsResponse : ListResponseBase<WidgetModel>
    {
    }
}